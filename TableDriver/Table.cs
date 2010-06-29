using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace TableDriver
{
	public class Table
	{
		/**
		* Create a table in the database given a list of fields and the 
		* table name. We use a Field collection as a conveneint way of specifying all
		* field info including data type.
		*/
		public static void CreateTable( string in_tableName, List<Field> in_fields, DbConfig in_config )
		{
			// sqlColumns is the portion of the create script that defines the columns
			string sqlColumns = "";

			for( int i = 0; i < in_fields.Count; i++ )
			{
				// this is sort of a hack .. but sql won't let you have duplicate column names
				// anyway, so we exclude the id column from our list, we hard code it into the query
				// since we want it to autonumber
				if( in_fields[ i ].Name != "ID" )
				{
					// map .net types to sql datatypes as strings for use in the create script
					string sqlType;
					if( in_fields[ i ].Type == typeof( String ) ) {
						sqlType = "varchar(20)";
					}
					else if( in_fields[ i ].Type == typeof( int ) ) {
						sqlType = "int";
					}
					else {
						throw new Exception( "Unsupported datatype: " + in_fields[ i ].Type.ToString() );
					}

					sqlColumns += in_fields[ i ].Name + " " + sqlType + " not null";
					if( i != in_fields.Count - 1 ) {
						sqlColumns += ", ";
					}
				}
			}

			// hack for special case where we don't have any properties
			string firstSeparator = ", ";
			if( in_fields.Count == 0 ) {
				firstSeparator = " ";
			}

			using( IDbConnection dbConnection = ConnectionFactory.GetConnection( in_config ) ) {
				IDataReader reader = DbUtil.ExecuteReader(
					dbConnection,
					"create table [" + in_config.Schema + "].[" + in_tableName + "] ( ID int identity(0,1) not null" + firstSeparator + sqlColumns + " )" );
				reader.Close();
			}
		}

		// TODO: put this function somewhere else besides Table - shouldn't be here
		public static void RenameColumn(
			IDbConnection in_dbConnection, string in_schema, string in_table,
			string in_oldColumnName, string in_newColumnName )
		{
			// TODO: fix to use qualified name mechanism
			// string qualifiedTableName = WriteQualifiedName( in_schema, in_table );
			string sqlText = @"
					exec sp_rename 
						[" + in_schema + "].[" + in_table + "].[" + in_oldColumnName + @"],
						[" + in_schema + "].[" + in_table + "].[" + in_newColumnName + @"]
				";
		}

	

		/**
		* Check for existence of database table
		*/
		public static bool Exists( IDbConnection in_dbConnection, string in_schema, string in_table )
		{
			bool bReturnValue = false;
			string sqlCommand = @"
				select * from information_schema.tables 
				where TABLE_SCHEMA = '" + in_schema + @"' 
				and TABLE_NAME = '" + in_table + "'";
			IDataReader reader;
			reader = DbUtil.ExecuteReader( in_dbConnection, sqlCommand );
			bReturnValue = reader.Read();
			reader.Close();

			return bReturnValue;
		}

		/**
		* Drops a database table given the table name and schema
		*/
		public static void Drop( string in_schema, string in_table, DbConfig in_config )
		{
			using( IDbConnection dbConnection = ConnectionFactory.GetConnection( in_config ) )
			{
				dbConnection.Open();
				IDataReader reader;
				reader = DbUtil.ExecuteReader( dbConnection, "drop table [" + in_schema + "].[" + in_table + "]" );
				reader.Close();
			}
		}
	}
}
