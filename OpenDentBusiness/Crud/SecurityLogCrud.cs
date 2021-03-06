//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class SecurityLogCrud {
		///<summary>Gets one SecurityLog object from the database using the primary key.  Returns null if not found.</summary>
		public static SecurityLog SelectOne(long securityLogNum){
			string command="SELECT * FROM securitylog "
				+"WHERE SecurityLogNum = "+POut.Long(securityLogNum);
			List<SecurityLog> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SecurityLog object from the database using a query.</summary>
		public static SecurityLog SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SecurityLog> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SecurityLog objects from the database using a query.</summary>
		public static List<SecurityLog> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SecurityLog> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<SecurityLog> TableToList(DataTable table){
			List<SecurityLog> retVal=new List<SecurityLog>();
			SecurityLog securityLog;
			foreach(DataRow row in table.Rows) {
				securityLog=new SecurityLog();
				securityLog.SecurityLogNum= PIn.Long  (row["SecurityLogNum"].ToString());
				securityLog.PermType      = (OpenDentBusiness.Permissions)PIn.Int(row["PermType"].ToString());
				securityLog.UserNum       = PIn.Long  (row["UserNum"].ToString());
				securityLog.LogDateTime   = PIn.DateT (row["LogDateTime"].ToString());
				securityLog.LogText       = PIn.String(row["LogText"].ToString());
				securityLog.PatNum        = PIn.Long  (row["PatNum"].ToString());
				securityLog.CompName      = PIn.String(row["CompName"].ToString());
				securityLog.FKey          = PIn.Long  (row["FKey"].ToString());
				securityLog.LogSource     = (OpenDentBusiness.LogSources)PIn.Int(row["LogSource"].ToString());
				securityLog.DefNum        = PIn.Long  (row["DefNum"].ToString());
				retVal.Add(securityLog);
			}
			return retVal;
		}

		///<summary>Converts a list of SecurityLog into a DataTable.</summary>
		public static DataTable ListToTable(List<SecurityLog> listSecurityLogs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="SecurityLog";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("SecurityLogNum");
			table.Columns.Add("PermType");
			table.Columns.Add("UserNum");
			table.Columns.Add("LogDateTime");
			table.Columns.Add("LogText");
			table.Columns.Add("PatNum");
			table.Columns.Add("CompName");
			table.Columns.Add("FKey");
			table.Columns.Add("LogSource");
			table.Columns.Add("DefNum");
			foreach(SecurityLog securityLog in listSecurityLogs) {
				table.Rows.Add(new object[] {
					POut.Long  (securityLog.SecurityLogNum),
					POut.Int   ((int)securityLog.PermType),
					POut.Long  (securityLog.UserNum),
					POut.DateT (securityLog.LogDateTime,false),
					            securityLog.LogText,
					POut.Long  (securityLog.PatNum),
					            securityLog.CompName,
					POut.Long  (securityLog.FKey),
					POut.Int   ((int)securityLog.LogSource),
					POut.Long  (securityLog.DefNum),
				});
			}
			return table;
		}

		///<summary>Inserts one SecurityLog into the database.  Returns the new priKey.</summary>
		public static long Insert(SecurityLog securityLog){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				securityLog.SecurityLogNum=DbHelper.GetNextOracleKey("securitylog","SecurityLogNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(securityLog,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							securityLog.SecurityLogNum++;
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
				return Insert(securityLog,false);
			}
		}

		///<summary>Inserts one SecurityLog into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(SecurityLog securityLog,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				securityLog.SecurityLogNum=ReplicationServers.GetKey("securitylog","SecurityLogNum");
			}
			string command="INSERT INTO securitylog (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SecurityLogNum,";
			}
			command+="PermType,UserNum,LogDateTime,LogText,PatNum,CompName,FKey,LogSource,DefNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(securityLog.SecurityLogNum)+",";
			}
			command+=
				     POut.Int   ((int)securityLog.PermType)+","
				+    POut.Long  (securityLog.UserNum)+","
				+    DbHelper.Now()+","
				+"'"+POut.String(securityLog.LogText)+"',"
				+    POut.Long  (securityLog.PatNum)+","
				+"'"+POut.String(securityLog.CompName)+"',"
				+    POut.Long  (securityLog.FKey)+","
				+    POut.Int   ((int)securityLog.LogSource)+","
				+    POut.Long  (securityLog.DefNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				securityLog.SecurityLogNum=Db.NonQ(command,true);
			}
			return securityLog.SecurityLogNum;
		}

		///<summary>Inserts one SecurityLog into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SecurityLog securityLog){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(securityLog,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					securityLog.SecurityLogNum=DbHelper.GetNextOracleKey("securitylog","SecurityLogNum"); //Cacheless method
				}
				return InsertNoCache(securityLog,true);
			}
		}

		///<summary>Inserts one SecurityLog into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SecurityLog securityLog,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO securitylog (";
			if(!useExistingPK && isRandomKeys) {
				securityLog.SecurityLogNum=ReplicationServers.GetKeyNoCache("securitylog","SecurityLogNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="SecurityLogNum,";
			}
			command+="PermType,UserNum,LogDateTime,LogText,PatNum,CompName,FKey,LogSource,DefNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(securityLog.SecurityLogNum)+",";
			}
			command+=
				     POut.Int   ((int)securityLog.PermType)+","
				+    POut.Long  (securityLog.UserNum)+","
				+    DbHelper.Now()+","
				+"'"+POut.String(securityLog.LogText)+"',"
				+    POut.Long  (securityLog.PatNum)+","
				+"'"+POut.String(securityLog.CompName)+"',"
				+    POut.Long  (securityLog.FKey)+","
				+    POut.Int   ((int)securityLog.LogSource)+","
				+    POut.Long  (securityLog.DefNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				securityLog.SecurityLogNum=Db.NonQ(command,true);
			}
			return securityLog.SecurityLogNum;
		}

		///<summary>Updates one SecurityLog in the database.</summary>
		public static void Update(SecurityLog securityLog){
			string command="UPDATE securitylog SET "
				+"PermType      =  "+POut.Int   ((int)securityLog.PermType)+", "
				+"UserNum       =  "+POut.Long  (securityLog.UserNum)+", "
				//LogDateTime not allowed to change
				+"LogText       = '"+POut.String(securityLog.LogText)+"', "
				+"PatNum        =  "+POut.Long  (securityLog.PatNum)+", "
				+"CompName      = '"+POut.String(securityLog.CompName)+"', "
				+"FKey          =  "+POut.Long  (securityLog.FKey)+", "
				+"LogSource     =  "+POut.Int   ((int)securityLog.LogSource)+", "
				+"DefNum        =  "+POut.Long  (securityLog.DefNum)+" "
				+"WHERE SecurityLogNum = "+POut.Long(securityLog.SecurityLogNum);
			Db.NonQ(command);
		}

		///<summary>Updates one SecurityLog in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(SecurityLog securityLog,SecurityLog oldSecurityLog){
			string command="";
			if(securityLog.PermType != oldSecurityLog.PermType) {
				if(command!=""){ command+=",";}
				command+="PermType = "+POut.Int   ((int)securityLog.PermType)+"";
			}
			if(securityLog.UserNum != oldSecurityLog.UserNum) {
				if(command!=""){ command+=",";}
				command+="UserNum = "+POut.Long(securityLog.UserNum)+"";
			}
			//LogDateTime not allowed to change
			if(securityLog.LogText != oldSecurityLog.LogText) {
				if(command!=""){ command+=",";}
				command+="LogText = '"+POut.String(securityLog.LogText)+"'";
			}
			if(securityLog.PatNum != oldSecurityLog.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(securityLog.PatNum)+"";
			}
			if(securityLog.CompName != oldSecurityLog.CompName) {
				if(command!=""){ command+=",";}
				command+="CompName = '"+POut.String(securityLog.CompName)+"'";
			}
			if(securityLog.FKey != oldSecurityLog.FKey) {
				if(command!=""){ command+=",";}
				command+="FKey = "+POut.Long(securityLog.FKey)+"";
			}
			if(securityLog.LogSource != oldSecurityLog.LogSource) {
				if(command!=""){ command+=",";}
				command+="LogSource = "+POut.Int   ((int)securityLog.LogSource)+"";
			}
			if(securityLog.DefNum != oldSecurityLog.DefNum) {
				if(command!=""){ command+=",";}
				command+="DefNum = "+POut.Long(securityLog.DefNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE securitylog SET "+command
				+" WHERE SecurityLogNum = "+POut.Long(securityLog.SecurityLogNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(SecurityLog,SecurityLog) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(SecurityLog securityLog,SecurityLog oldSecurityLog) {
			if(securityLog.PermType != oldSecurityLog.PermType) {
				return true;
			}
			if(securityLog.UserNum != oldSecurityLog.UserNum) {
				return true;
			}
			//LogDateTime not allowed to change
			if(securityLog.LogText != oldSecurityLog.LogText) {
				return true;
			}
			if(securityLog.PatNum != oldSecurityLog.PatNum) {
				return true;
			}
			if(securityLog.CompName != oldSecurityLog.CompName) {
				return true;
			}
			if(securityLog.FKey != oldSecurityLog.FKey) {
				return true;
			}
			if(securityLog.LogSource != oldSecurityLog.LogSource) {
				return true;
			}
			if(securityLog.DefNum != oldSecurityLog.DefNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one SecurityLog from the database.</summary>
		public static void Delete(long securityLogNum){
			string command="DELETE FROM securitylog "
				+"WHERE SecurityLogNum = "+POut.Long(securityLogNum);
			Db.NonQ(command);
		}

	}
}