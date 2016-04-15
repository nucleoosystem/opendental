//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class LabPanelCrud {
		///<summary>Gets one LabPanel object from the database using the primary key.  Returns null if not found.</summary>
		public static LabPanel SelectOne(long labPanelNum){
			string command="SELECT * FROM labpanel "
				+"WHERE LabPanelNum = "+POut.Long(labPanelNum);
			List<LabPanel> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one LabPanel object from the database using a query.</summary>
		public static LabPanel SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<LabPanel> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of LabPanel objects from the database using a query.</summary>
		public static List<LabPanel> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<LabPanel> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<LabPanel> TableToList(DataTable table){
			List<LabPanel> retVal=new List<LabPanel>();
			LabPanel labPanel;
			foreach(DataRow row in table.Rows) {
				labPanel=new LabPanel();
				labPanel.LabPanelNum      = PIn.Long  (row["LabPanelNum"].ToString());
				labPanel.PatNum           = PIn.Long  (row["PatNum"].ToString());
				labPanel.RawMessage       = PIn.String(row["RawMessage"].ToString());
				labPanel.LabNameAddress   = PIn.String(row["LabNameAddress"].ToString());
				labPanel.DateTStamp       = PIn.DateT (row["DateTStamp"].ToString());
				labPanel.SpecimenCondition= PIn.String(row["SpecimenCondition"].ToString());
				labPanel.SpecimenSource   = PIn.String(row["SpecimenSource"].ToString());
				labPanel.ServiceId        = PIn.String(row["ServiceId"].ToString());
				labPanel.ServiceName      = PIn.String(row["ServiceName"].ToString());
				labPanel.MedicalOrderNum  = PIn.Long  (row["MedicalOrderNum"].ToString());
				retVal.Add(labPanel);
			}
			return retVal;
		}

		///<summary>Converts a list of LabPanel into a DataTable.</summary>
		public static DataTable ListToTable(List<LabPanel> listLabPanels,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="LabPanel";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("LabPanelNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("RawMessage");
			table.Columns.Add("LabNameAddress");
			table.Columns.Add("DateTStamp");
			table.Columns.Add("SpecimenCondition");
			table.Columns.Add("SpecimenSource");
			table.Columns.Add("ServiceId");
			table.Columns.Add("ServiceName");
			table.Columns.Add("MedicalOrderNum");
			foreach(LabPanel labPanel in listLabPanels) {
				table.Rows.Add(new object[] {
					POut.Long  (labPanel.LabPanelNum),
					POut.Long  (labPanel.PatNum),
					            labPanel.RawMessage,
					            labPanel.LabNameAddress,
					POut.DateT (labPanel.DateTStamp,false),
					            labPanel.SpecimenCondition,
					            labPanel.SpecimenSource,
					            labPanel.ServiceId,
					            labPanel.ServiceName,
					POut.Long  (labPanel.MedicalOrderNum),
				});
			}
			return table;
		}

		///<summary>Inserts one LabPanel into the database.  Returns the new priKey.</summary>
		public static long Insert(LabPanel labPanel){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				labPanel.LabPanelNum=DbHelper.GetNextOracleKey("labpanel","LabPanelNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(labPanel,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							labPanel.LabPanelNum++;
							loopcount++;
						}
						else{
							throw ex;
						}
					}
				}
				throw new ApplicationException("Insert failed.  Could not generate primary key.");
			}
			else {
				return Insert(labPanel,false);
			}
		}

		///<summary>Inserts one LabPanel into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(LabPanel labPanel,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				labPanel.LabPanelNum=ReplicationServers.GetKey("labpanel","LabPanelNum");
			}
			string command="INSERT INTO labpanel (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="LabPanelNum,";
			}
			command+="PatNum,RawMessage,LabNameAddress,SpecimenCondition,SpecimenSource,ServiceId,ServiceName,MedicalOrderNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(labPanel.LabPanelNum)+",";
			}
			command+=
				     POut.Long  (labPanel.PatNum)+","
				+    DbHelper.ParamChar+"paramRawMessage,"
				+"'"+POut.String(labPanel.LabNameAddress)+"',"
				//DateTStamp can only be set by MySQL
				+"'"+POut.String(labPanel.SpecimenCondition)+"',"
				+"'"+POut.String(labPanel.SpecimenSource)+"',"
				+"'"+POut.String(labPanel.ServiceId)+"',"
				+"'"+POut.String(labPanel.ServiceName)+"',"
				+    POut.Long  (labPanel.MedicalOrderNum)+")";
			if(labPanel.RawMessage==null) {
				labPanel.RawMessage="";
			}
			OdSqlParameter paramRawMessage=new OdSqlParameter("paramRawMessage",OdDbType.Text,labPanel.RawMessage);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramRawMessage);
			}
			else {
				labPanel.LabPanelNum=Db.NonQ(command,true,paramRawMessage);
			}
			return labPanel.LabPanelNum;
		}

		///<summary>Inserts one LabPanel into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(LabPanel labPanel){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(labPanel,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					labPanel.LabPanelNum=DbHelper.GetNextOracleKey("labpanel","LabPanelNum"); //Cacheless method
				}
				return InsertNoCache(labPanel,true);
			}
		}

		///<summary>Inserts one LabPanel into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(LabPanel labPanel,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO labpanel (";
			if(!useExistingPK && isRandomKeys) {
				labPanel.LabPanelNum=ReplicationServers.GetKeyNoCache("labpanel","LabPanelNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="LabPanelNum,";
			}
			command+="PatNum,RawMessage,LabNameAddress,SpecimenCondition,SpecimenSource,ServiceId,ServiceName,MedicalOrderNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(labPanel.LabPanelNum)+",";
			}
			command+=
				     POut.Long  (labPanel.PatNum)+","
				+    DbHelper.ParamChar+"paramRawMessage,"
				+"'"+POut.String(labPanel.LabNameAddress)+"',"
				//DateTStamp can only be set by MySQL
				+"'"+POut.String(labPanel.SpecimenCondition)+"',"
				+"'"+POut.String(labPanel.SpecimenSource)+"',"
				+"'"+POut.String(labPanel.ServiceId)+"',"
				+"'"+POut.String(labPanel.ServiceName)+"',"
				+    POut.Long  (labPanel.MedicalOrderNum)+")";
			if(labPanel.RawMessage==null) {
				labPanel.RawMessage="";
			}
			OdSqlParameter paramRawMessage=new OdSqlParameter("paramRawMessage",OdDbType.Text,labPanel.RawMessage);
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramRawMessage);
			}
			else {
				labPanel.LabPanelNum=Db.NonQ(command,true,paramRawMessage);
			}
			return labPanel.LabPanelNum;
		}

		///<summary>Updates one LabPanel in the database.</summary>
		public static void Update(LabPanel labPanel){
			string command="UPDATE labpanel SET "
				+"PatNum           =  "+POut.Long  (labPanel.PatNum)+", "
				+"RawMessage       =  "+DbHelper.ParamChar+"paramRawMessage, "
				+"LabNameAddress   = '"+POut.String(labPanel.LabNameAddress)+"', "
				//DateTStamp can only be set by MySQL
				+"SpecimenCondition= '"+POut.String(labPanel.SpecimenCondition)+"', "
				+"SpecimenSource   = '"+POut.String(labPanel.SpecimenSource)+"', "
				+"ServiceId        = '"+POut.String(labPanel.ServiceId)+"', "
				+"ServiceName      = '"+POut.String(labPanel.ServiceName)+"', "
				+"MedicalOrderNum  =  "+POut.Long  (labPanel.MedicalOrderNum)+" "
				+"WHERE LabPanelNum = "+POut.Long(labPanel.LabPanelNum);
			if(labPanel.RawMessage==null) {
				labPanel.RawMessage="";
			}
			OdSqlParameter paramRawMessage=new OdSqlParameter("paramRawMessage",OdDbType.Text,labPanel.RawMessage);
			Db.NonQ(command,paramRawMessage);
		}

		///<summary>Updates one LabPanel in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(LabPanel labPanel,LabPanel oldLabPanel){
			string command="";
			if(labPanel.PatNum != oldLabPanel.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(labPanel.PatNum)+"";
			}
			if(labPanel.RawMessage != oldLabPanel.RawMessage) {
				if(command!=""){ command+=",";}
				command+="RawMessage = "+DbHelper.ParamChar+"paramRawMessage";
			}
			if(labPanel.LabNameAddress != oldLabPanel.LabNameAddress) {
				if(command!=""){ command+=",";}
				command+="LabNameAddress = '"+POut.String(labPanel.LabNameAddress)+"'";
			}
			//DateTStamp can only be set by MySQL
			if(labPanel.SpecimenCondition != oldLabPanel.SpecimenCondition) {
				if(command!=""){ command+=",";}
				command+="SpecimenCondition = '"+POut.String(labPanel.SpecimenCondition)+"'";
			}
			if(labPanel.SpecimenSource != oldLabPanel.SpecimenSource) {
				if(command!=""){ command+=",";}
				command+="SpecimenSource = '"+POut.String(labPanel.SpecimenSource)+"'";
			}
			if(labPanel.ServiceId != oldLabPanel.ServiceId) {
				if(command!=""){ command+=",";}
				command+="ServiceId = '"+POut.String(labPanel.ServiceId)+"'";
			}
			if(labPanel.ServiceName != oldLabPanel.ServiceName) {
				if(command!=""){ command+=",";}
				command+="ServiceName = '"+POut.String(labPanel.ServiceName)+"'";
			}
			if(labPanel.MedicalOrderNum != oldLabPanel.MedicalOrderNum) {
				if(command!=""){ command+=",";}
				command+="MedicalOrderNum = "+POut.Long(labPanel.MedicalOrderNum)+"";
			}
			if(command==""){
				return false;
			}
			if(labPanel.RawMessage==null) {
				labPanel.RawMessage="";
			}
			OdSqlParameter paramRawMessage=new OdSqlParameter("paramRawMessage",OdDbType.Text,labPanel.RawMessage);
			command="UPDATE labpanel SET "+command
				+" WHERE LabPanelNum = "+POut.Long(labPanel.LabPanelNum);
			Db.NonQ(command,paramRawMessage);
			return true;
		}

		///<summary>Returns true if Update(LabPanel,LabPanel) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(LabPanel labPanel,LabPanel oldLabPanel) {
			if(labPanel.PatNum != oldLabPanel.PatNum) {
				return true;
			}
			if(labPanel.RawMessage != oldLabPanel.RawMessage) {
				return true;
			}
			if(labPanel.LabNameAddress != oldLabPanel.LabNameAddress) {
				return true;
			}
			//DateTStamp can only be set by MySQL
			if(labPanel.SpecimenCondition != oldLabPanel.SpecimenCondition) {
				return true;
			}
			if(labPanel.SpecimenSource != oldLabPanel.SpecimenSource) {
				return true;
			}
			if(labPanel.ServiceId != oldLabPanel.ServiceId) {
				return true;
			}
			if(labPanel.ServiceName != oldLabPanel.ServiceName) {
				return true;
			}
			if(labPanel.MedicalOrderNum != oldLabPanel.MedicalOrderNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one LabPanel from the database.</summary>
		public static void Delete(long labPanelNum){
			string command="DELETE FROM labpanel "
				+"WHERE LabPanelNum = "+POut.Long(labPanelNum);
			Db.NonQ(command);
		}

	}
}