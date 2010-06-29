using System;
using System.Data.SqlClient;
using System.Data;


namespace TableDriver
{
	/// <summary>
	/// Low level utilities for database access
	/// Currently just various wrappers around ExecutReader
	/// </summary>
	public class DbUtil
	{
		/// <summary>
		/// Returns a datareader containing no rows of data. Used to get the field names
		/// and types from database tables that implement an entity.
		/// </summary>
		/// <param name="in_sqlConnection">Database connection used to make the call</param>
		/// <param name="in_sqlText">SQL query string</param>
		/// <returns></returns>
        public static IDataReader ExecuteReaderSchemaOnly( IDbConnection in_dbConnection, string in_sqlText )
        {
			bool schemaOnly = true;
			return ExecuteReader( in_dbConnection, in_sqlText, schemaOnly );
        }

		/// <summary>
		/// Return datareader containing data from the database described by in_dbConnection.
		/// </summary>
		/// <param name="in_dbConnection">Database connection used to make the call</param>
		/// <param name="in_sqlText">SQL query string</param>
		/// <returns></returns>
		public static IDataReader ExecuteReader( IDbConnection in_dbConnection, string in_sqlText )
		{
			bool schemaOnly = false;
			return ExecuteReader( in_dbConnection, in_sqlText, schemaOnly );
		}

		/// <summary>
		/// Core method for returning data readers.  This is private to Util, only used by ExecuteReader
		/// and ExecuteReaderSchemaOnly
		/// </summary>
		/// <param name="in_dbConnection">Database connection to make db call</param>
		/// <param name="in_sqlText">SQL query string</param>
		/// <param name="in_schemaOnly">return only schema if true, return data normally otw</param>
		/// <returns></returns>
        private static IDataReader ExecuteReader( IDbConnection in_dbConnection, string in_sqlText, bool in_schemaOnly )
        {
			if( in_dbConnection.State == ConnectionState.Closed ) {
				in_dbConnection.Open();
			}
			if( in_dbConnection.State == ConnectionState.Open ) {
				IDataReader sqlReader;
				IDbCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = (SqlConnection)in_dbConnection;
				sqlCommand.CommandText = in_sqlText;
				if( in_schemaOnly == true ) {
					sqlReader = sqlCommand.ExecuteReader( CommandBehavior.SchemaOnly );
				}
				else {
					sqlReader = sqlCommand.ExecuteReader();
				}
				return sqlReader;
			}
			else {
				throw new Exception( "Sql connection not in open state, cannot execute reader" );
			}
        }
	}
}
