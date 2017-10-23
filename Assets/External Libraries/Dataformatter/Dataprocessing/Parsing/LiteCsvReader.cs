using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Dataformatter.Dataprocessing.Parsing
{
    /// <summary>
    /// Determines how empty lines are interpreted when reading CSV files.
    /// These values do not affect empty lines that occur within quoted fields
    /// or empty lines that appear at the end of the input file.
    /// </summary>
    public enum EmptyLineBehavior
    {
        /// <summary>
        /// Empty lines are interpreted as a line with zero columns.
        /// </summary>
        NoColumns,

        /// <summary>
        /// Empty lines are interpreted as a line with a single empty column.
        /// </summary>
        EmptyColumn,

        /// <summary>
        /// Empty lines are skipped over as though they did not exist.
        /// </summary>
        Ignore,

        /// <summary>
        /// An empty line is interpreted as the end of the input file.
        /// </summary>
        EndOfFile,
    }

    /// <summary>
    /// Common base class for CSV reader and writer classes.
    /// </summary>
    public abstract class CsvFileCommon
    {
        /// <summary>
        /// These are special characters in CSV files. If a column contains any
        /// of these characters, the entire column is wrapped in double quotes.
        /// </summary>
        private readonly char[] _specialChars = {',', '"', '\r', '\n'};

        // Indexes into SpecialChars for characters with specific meaning
        private const int DelimiterIndex = 0;

        private const int QuoteIndex = 1;

        /// <summary>
        /// Gets/sets the character used for column delimiters.
        /// </summary>
        protected char Delimiter
        {
            get { return _specialChars[DelimiterIndex]; }
            set { _specialChars[DelimiterIndex] = value; }
        }

        /// <summary>
        /// Gets/sets the character used for column quotes.
        /// </summary>
        protected char Quote
        {
            get { return _specialChars[QuoteIndex]; }
            set { _specialChars[QuoteIndex] = value; }
        }
    }

    /// <summary>
    /// Class for reading from comma-separated-value (CSV) files
    /// </summary>
    public class CsvFileReader : CsvFileCommon, IDisposable
    {
        // Private members
        private readonly StreamReader _reader;

        private string _currLine;
        private int _currPos;
        private readonly EmptyLineBehavior _emptyLineBehavior;

        /// <summary>
        /// Initializes a new instance of the CsvFileReader class for the
        /// specified stream.
        /// </summary>
        /// <param name="reader">The stream to read from</param>
        /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
        public CsvFileReader(StreamReader reader,
            EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
        {
            _reader = reader;
            _emptyLineBehavior = emptyLineBehavior;
        }

//        /// <summary>
//        /// Initializes a new instance of the CsvFileReader class for the
//        /// specified stream.
//        /// </summary>
//        /// <param name="stream">The stream to read from</param>
//        /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
//        public CsvFileReader(Stream stream,
//                         EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
//        { 
//            Reader = new StreamReader(stream);
//            EmptyLineBehavior = emptyLineBehavior;
//        }

        /// <summary>
        /// Initializes a new instance of the CsvFileReader class for the
        /// specified file path.
        /// </summary>
        /// <param name="path">The name of the CSV file to read from</param>
        /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
        public CsvFileReader(string path,
            EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
        {
            var stream = new FileStream(path, FileMode.Open);
            _reader = new StreamReader(stream);
//            Reader = new StreamReader(path);
            _emptyLineBehavior = emptyLineBehavior;
        }

//        public static List<List<string>> ReadAll(string path, Encoding encoding)
//        {
//            using (var sr = new StreamReader(path, encoding))
//            {
//                var cfr = new CsvFileReader(sr);
//                List<List<string>> dataGrid = new List<List<string>>();
//                if (cfr.ReadAll(dataGrid)) return dataGrid;
//            }
//            return null;
//        }

        public bool ReadAll(List<List<string>> dataGrid)
        {
            // Verify required argument
            if (dataGrid == null)
            {
                throw new ArgumentNullException(dataGrid.ToString());
            }

            var row = new List<string>();
            while (ReadRow(row))
            {
                dataGrid.Add(new List<string>(row));
            }

            return true;
        }

        /// <summary>
        /// Reads a row of columns from the current CSV file. Returns false if no
        /// more data could be read because the end of the file was reached.
        /// </summary>
        /// <param name="columns">Collection to hold the columns read</param>
        public bool ReadRow(List<string> columns)
        {
            // Verify required argument
            if (columns == null)
                throw new ArgumentNullException(columns.ToString());

            ReadNextLine:
            // Read next line from the file
            _currLine = _reader.ReadLine();
            _currPos = 0;
            // Test for end of file
            if (_currLine == null)
                return false;
            // Test for empty line
            if (_currLine.Length == 0)
            {
                switch (_emptyLineBehavior)
                {
                    case EmptyLineBehavior.NoColumns:
                        columns.Clear();
                        return true;
                    case EmptyLineBehavior.Ignore:
                        goto ReadNextLine;
                    case EmptyLineBehavior.EndOfFile:
                        return false;
                }
            }

            // Parse line
            var numColumns = 0;
            while (true)
            {
                // Read next column
                string column;
                if (_currPos < _currLine.Length && _currLine[_currPos] == Quote)
                    column = ReadQuotedColumn();
                else
                    column = ReadUnquotedColumn();
                // Add column to list
                if (numColumns < columns.Count)
                    columns[numColumns] = column;
                else
                    columns.Add(column);
                numColumns++;
                // Break if we reached the end of the line
                if (_currLine == null || _currPos == _currLine.Length)
                    break;
                // Otherwise skip delimiter
                Debug.Assert(_currLine[_currPos] == Delimiter);
                _currPos++;
            }
            // Remove any unused columns from collection
            if (numColumns < columns.Count)
                columns.RemoveRange(numColumns, columns.Count - numColumns);
            // Indicate success
            return true;
        }

        /// <summary>
        /// Reads a quoted column by reading from the current line until a
        /// closing quote is found or the end of the file is reached. On return,
        /// the current position points to the delimiter or the end of the last
        /// line in the file. Note: CurrLine may be set to null on return.
        /// </summary>
        private string ReadQuotedColumn()
        {
            // Skip opening quote character
            Debug.Assert(_currPos < _currLine.Length && _currLine[_currPos] == Quote);
            _currPos++;

            // Parse column
            var builder = new StringBuilder();
            while (true)
            {
                while (_currPos == _currLine.Length)
                {
                    // End of line so attempt to read the next line
                    _currLine = _reader.ReadLine();
                    _currPos = 0;
                    // Done if we reached the end of the file
                    if (_currLine == null)
                        return builder.ToString();
                    // Otherwise, treat as a multi-line field
                    builder.Append(Environment.NewLine);
                }

                // Test for quote character
                if (_currLine[_currPos] == Quote)
                {
                    // If two quotes, skip first and treat second as literal
                    var nextPos = (_currPos + 1);
                    if (nextPos < _currLine.Length && _currLine[nextPos] == Quote)
                        _currPos++;
                    else
                        break; // Single quote ends quoted sequence
                }
                // Add current character to the column
                builder.Append(_currLine[_currPos++]);
            }

            if (_currPos < _currLine.Length)
            {
                // Consume closing quote
                Debug.Assert(_currLine[_currPos] == Quote);
                _currPos++;
                // Append any additional characters appearing before next delimiter
                builder.Append(ReadUnquotedColumn());
            }
            // Return column value
            return builder.ToString();
        }

        /// <summary>
        /// Reads an unquoted column by reading from the current line until a
        /// delimiter is found or the end of the line is reached. On return, the
        /// current position points to the delimiter or the end of the current
        /// line.
        /// </summary>
        private string ReadUnquotedColumn()
        {
            var startPos = _currPos;
            _currPos = _currLine.IndexOf(Delimiter, _currPos);
            if (_currPos == -1)
                _currPos = _currLine.Length;
            if (_currPos > startPos)
                return _currLine.Substring(startPos, _currPos - startPos);
            return String.Empty;
        }

        // Propagate Dispose to StreamReader
        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}