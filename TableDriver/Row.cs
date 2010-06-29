using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace TableDriver
{
	/**
	 * Row encapsulates low-level sql operations on entity instances
	 */
	public class Row
	{
		// retrieve objects by single field comparison
		// TODO: should this be a generic method? Then we can return List<T> directly
		public static List<object> GetByField( Type in_type, string in_field, string in_value ) {
			DbConfig config = new DbConfig();
			string qualifiedTableName = SqlUtil.WriteQualifiedName( config.Schema, in_type.Name );
			string qualifiedFieldName = SqlUtil.WriteQualifiedName( in_field );
			string sqlString = "select * from " + qualifiedTableName + " where " + qualifiedFieldName + " = '" + in_value + "'";
			IDataReader rdr = DbUtil.ExecuteReader( ConnectionFactory.GetConnection( new DbConfig() ), sqlString );
			
			List<object> retval = new List<object>();
			while( rdr.Read() ) {
				object obj = Activator.CreateInstance( in_type );
				PopulateObjectFromReader( obj, rdr );
				retval.Add( obj );
			}
			return retval;
		}

		private static void PopulateObjectFromReader( object obj, IDataReader reader ) {
			PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties( obj );
			for( int i = 0; i < descriptors.Count; i++ ) {
				object value = reader[ descriptors[ i ].Name ];
				descriptors[ i ].SetValue( obj, value );
			}
		}

		// TODO: only works with string datatypes
		public static void Insert( object obj ) {
			DbConfig config = new DbConfig();
			string tableName = obj.GetType().Name;
			string schemaName = config.Schema;
			string fields = SqlUtil.BuildFieldNames( obj );
			string values = SqlUtil.BuildFieldValues( obj );

			/*
			PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties( obj );
			for( int i = 0; i < descriptors.Count; i++ ) {
				Console.WriteLine( descriptors[ i ].Name + ": " + descriptors[i].GetValue( obj ).ToString() );
				fields += "[" + descriptors[i].Name + "]";
				values += "'" + descriptors[ i ].GetValue( obj ) + "'";
				if( i < descriptors.Count - 1 ) {
					fields += ",";
					values += ",";
				}
			}
			*/
			string qualifiedTableName = SqlUtil.WriteQualifiedName( schemaName, tableName );
			string sqlString = "insert into " + qualifiedTableName + 
				" ( " + fields + 
				" ) values ( " + values + " ) ";
			Console.WriteLine( sqlString );

			IDbConnection conn = ConnectionFactory.GetConnection( new DbConfig() );
			using( conn ) {
				IDataReader rdr = DbUtil.ExecuteReader( conn, sqlString );
				rdr.Close();
			}
		}
	} // class
} // namespace
