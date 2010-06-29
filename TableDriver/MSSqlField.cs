using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TableDriver
{
	/*
	 * Version of Field that uses sql data types. 
	 * TODO: create common class for all field types?
	 */
	public class MSSqlField
	{
		public string Name {
			get { return m_name; }
		} string m_name;

		public SqlDbType Type {
			get { return m_type; }
		} SqlDbType m_type;
	}
}
