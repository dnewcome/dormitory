using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace Djn.ORM
{
	public class Dormitory
	{
		/*
		 * Persist a single object to the database - note that there must be at least
		 * one property on the object - currently we don't check this (TODO: fix)
		 */
		public static void Persist( object obj )
		{
			// we do some convoluted stuff here involving fields, which is a construct
			// that is part of the table driver, in order to check that the table 
			// exists before we try to persist the obj instance. TODO: simplify this
			List<TableDriver.Field> fields = new List<TableDriver.Field>();
			foreach( PropertyDescriptor descriptor in TypeDescriptor.GetProperties( obj ) ) {
				Console.WriteLine( descriptor.Name + ": " + descriptor.GetValue( obj ).ToString() );
				fields.Add( new TableDriver.Field( descriptor.Name, descriptor.PropertyType ) );
			}
			TableDriver.DbConfig config = new TableDriver.DbConfig();
			IDbConnection conn = new SqlConnection( config.ConnectionString );
			conn.Open();
			using( conn ) {
				if( !TableDriver.Table.Exists( conn, config.Schema, obj.GetType().Name ) ) {
					TableDriver.Table.CreateTable( obj.GetType().Name, fields, config );
				}
			}
			TableDriver.Row.Insert( obj );
		}
	} // class
} // namespace
