using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TableDriver
{
	/**
	 * Utilities to abstract textual sql string manipulations
	 */
	class SqlUtil
	{
		/**
		* write out table name using bracket notation .. 
		*/
		public static string WriteQualifiedName( string in_schema, string in_table, string in_column ) {
			return "[" + in_schema + "].[" + in_table + "].[" + in_column + "]";
		}
		public static string WriteQualifiedName( string in_schema, string in_table ) {
			return "[" + in_schema + "].[" + in_table + "]";
		}
		public static string WriteQualifiedName( string in_schema ) {
			return "[" + in_schema + "]";
		}

		// Construct comma separated string of field names from given object
		// Note that we only find properties on the object instances not fields
		public static string BuildFieldNames( object obj ) {
			string fields = "";
			PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties( obj );
			for( int i = 0; i < descriptors.Count; i++ ) {
				fields += SqlUtil.WriteQualifiedName( descriptors[ i ].Name );
				if( i < descriptors.Count - 1 ) {
					fields += ",";
				}
			}
			return fields;
		}

		// Construct comma separated string of field values from given object
		// Note that we only find properties on the object instances not fields
		public static string BuildFieldValues( object obj ) {
			string values = "";
			PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties( obj );
			for( int i = 0; i < descriptors.Count; i++ ) {
				// 
				values += Stringify(
					descriptors[ i ].GetValue( obj ),
					descriptors[ i ].PropertyType
				);
				if( i < descriptors.Count - 1 ) {
					values += ",";
				}
			}
			return values;
		}

		// Take an object and return sql compatible string representation
		// TODO: can we use normal dispatch to get rid of RTTI?
		public static string Stringify( object in_value, Type in_type ) { 
			// TODO: should we be trying cast using 'is' or 'as' instead of using name?
			switch( in_type.Name ) {
				case "String":
					return STRING_DELIM + in_value + STRING_DELIM;
				case "DateTime":
					// TODO: adjust string formatting for database format
					return STRING_DELIM + in_value.ToString() + STRING_DELIM;
				case "Int32":
					return in_value.ToString();
				default:
					throw new Exception( "could not convert value to sql string" );
			}
		}
		public static readonly string STRING_DELIM = "'";
	} // class
} // namespace 
