///////////////////////////////////////////////////////////
//  AlarmContext.cs
//  Implementation of the Class AlarmContext
//  Generated by Enterprise Architect
//  Created on:      24-Sep-2019 4:45:44 PM
//  Original author: beh38
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Data.Entity;
using AlarmBase.DomainModel.Entities;
using System.Reflection;

namespace AlarmBase {
	public class AlarmContext<T> : DbContext {



		~AlarmContext(){

		}

		public AlarmContext(): base(){

		}

		/// 
		/// <param name="occ"></param>
		public void add(OccurenceLog<T> occ){

		}

		/// <summary>
		/// Copies the data of one object to another. The target object 'pulls' properties
		/// of the first. This any matching properties are written to the target.  The
		/// object copy is a shallow copy only. Any nested types will be copied as whole
		/// values rather than individual property assignments (ie. via assignment)
		/// </summary>
		/// <param name="source">The source object to copy from</param>
		/// <param name="target">The object to copy to</param>
		/// <param name="excludedProperties">A comma delimited list of properties that
		/// should not be copied</param>
		/// <param name="memberAccess">Reflection binding access</param>
		public static void CopyObjectData(object source, object target, string excludedProperties, BindingFlags memberAccess){

		}

		/// <summary>
		/// Deserialize an object from an XmlReader object.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="objectType"></param>
		public static object DeSerializeObject(XmlReader reader, Type objectType){

			return null;
		}

		/// 
		/// <param name="xml"></param>
		/// <param name="objectType"></param>
		public static object DeSerializeObject(string xml, Type objectType){

			return null;
		}

		public virtual DbSet<OccurenceLog<T>> occurencelog{
			get;
			set;
		}

		/// 
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder){

		}

		public virtual DbSet<RegisteredOccConfig<T>> registeredOccConfig{
			get;
			set;
		}

		/// 
		/// <param name="occ"></param>
		public void saveConfiges(OccurenceLog<T> occ){

		}

	}//end AlarmContext

}//end namespace AlarmBase