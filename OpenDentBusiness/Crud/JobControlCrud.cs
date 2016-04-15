//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class JobControlCrud {
		///<summary>Gets one JobControl object from the database using the primary key.  Returns null if not found.</summary>
		public static JobControl SelectOne(long jobControlNum){
			string command="SELECT * FROM jobcontrol "
				+"WHERE JobControlNum = "+POut.Long(jobControlNum);
			List<JobControl> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one JobControl object from the database using a query.</summary>
		public static JobControl SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<JobControl> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of JobControl objects from the database using a query.</summary>
		public static List<JobControl> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<JobControl> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<JobControl> TableToList(DataTable table){
			List<JobControl> retVal=new List<JobControl>();
			JobControl jobControl;
			foreach(DataRow row in table.Rows) {
				jobControl=new JobControl();
				jobControl.JobControlNum = PIn.Long  (row["JobControlNum"].ToString());
				jobControl.UserNum       = PIn.Long  (row["UserNum"].ToString());
				jobControl.JobControlType= (OpenDentBusiness.JobControlType)PIn.Int(row["JobControlType"].ToString());
				jobControl.ControlData   = PIn.String(row["ControlData"].ToString());
				jobControl.XPos          = PIn.Int   (row["XPos"].ToString());
				jobControl.YPos          = PIn.Int   (row["YPos"].ToString());
				jobControl.Width         = PIn.Int   (row["Width"].ToString());
				jobControl.Height        = PIn.Int   (row["Height"].ToString());
				retVal.Add(jobControl);
			}
			return retVal;
		}

		///<summary>Converts a list of JobControl into a DataTable.</summary>
		public static DataTable ListToTable(List<JobControl> listJobControls,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="JobControl";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("JobControlNum");
			table.Columns.Add("UserNum");
			table.Columns.Add("JobControlType");
			table.Columns.Add("ControlData");
			table.Columns.Add("XPos");
			table.Columns.Add("YPos");
			table.Columns.Add("Width");
			table.Columns.Add("Height");
			foreach(JobControl jobControl in listJobControls) {
				table.Rows.Add(new object[] {
					POut.Long  (jobControl.JobControlNum),
					POut.Long  (jobControl.UserNum),
					POut.Int   ((int)jobControl.JobControlType),
					            jobControl.ControlData,
					POut.Int   (jobControl.XPos),
					POut.Int   (jobControl.YPos),
					POut.Int   (jobControl.Width),
					POut.Int   (jobControl.Height),
				});
			}
			return table;
		}

		///<summary>Inserts one JobControl into the database.  Returns the new priKey.</summary>
		public static long Insert(JobControl jobControl){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				jobControl.JobControlNum=DbHelper.GetNextOracleKey("jobcontrol","JobControlNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(jobControl,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							jobControl.JobControlNum++;
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
				return Insert(jobControl,false);
			}
		}

		///<summary>Inserts one JobControl into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(JobControl jobControl,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				jobControl.JobControlNum=ReplicationServers.GetKey("jobcontrol","JobControlNum");
			}
			string command="INSERT INTO jobcontrol (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="JobControlNum,";
			}
			command+="UserNum,JobControlType,ControlData,XPos,YPos,Width,Height) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(jobControl.JobControlNum)+",";
			}
			command+=
				     POut.Long  (jobControl.UserNum)+","
				+    POut.Int   ((int)jobControl.JobControlType)+","
				+"'"+POut.String(jobControl.ControlData)+"',"
				+    POut.Int   (jobControl.XPos)+","
				+    POut.Int   (jobControl.YPos)+","
				+    POut.Int   (jobControl.Width)+","
				+    POut.Int   (jobControl.Height)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				jobControl.JobControlNum=Db.NonQ(command,true);
			}
			return jobControl.JobControlNum;
		}

		///<summary>Inserts one JobControl into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobControl jobControl){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(jobControl,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					jobControl.JobControlNum=DbHelper.GetNextOracleKey("jobcontrol","JobControlNum"); //Cacheless method
				}
				return InsertNoCache(jobControl,true);
			}
		}

		///<summary>Inserts one JobControl into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobControl jobControl,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO jobcontrol (";
			if(!useExistingPK && isRandomKeys) {
				jobControl.JobControlNum=ReplicationServers.GetKeyNoCache("jobcontrol","JobControlNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="JobControlNum,";
			}
			command+="UserNum,JobControlType,ControlData,XPos,YPos,Width,Height) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(jobControl.JobControlNum)+",";
			}
			command+=
				     POut.Long  (jobControl.UserNum)+","
				+    POut.Int   ((int)jobControl.JobControlType)+","
				+"'"+POut.String(jobControl.ControlData)+"',"
				+    POut.Int   (jobControl.XPos)+","
				+    POut.Int   (jobControl.YPos)+","
				+    POut.Int   (jobControl.Width)+","
				+    POut.Int   (jobControl.Height)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				jobControl.JobControlNum=Db.NonQ(command,true);
			}
			return jobControl.JobControlNum;
		}

		///<summary>Updates one JobControl in the database.</summary>
		public static void Update(JobControl jobControl){
			string command="UPDATE jobcontrol SET "
				+"UserNum       =  "+POut.Long  (jobControl.UserNum)+", "
				+"JobControlType=  "+POut.Int   ((int)jobControl.JobControlType)+", "
				+"ControlData   = '"+POut.String(jobControl.ControlData)+"', "
				+"XPos          =  "+POut.Int   (jobControl.XPos)+", "
				+"YPos          =  "+POut.Int   (jobControl.YPos)+", "
				+"Width         =  "+POut.Int   (jobControl.Width)+", "
				+"Height        =  "+POut.Int   (jobControl.Height)+" "
				+"WHERE JobControlNum = "+POut.Long(jobControl.JobControlNum);
			Db.NonQ(command);
		}

		///<summary>Updates one JobControl in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(JobControl jobControl,JobControl oldJobControl){
			string command="";
			if(jobControl.UserNum != oldJobControl.UserNum) {
				if(command!=""){ command+=",";}
				command+="UserNum = "+POut.Long(jobControl.UserNum)+"";
			}
			if(jobControl.JobControlType != oldJobControl.JobControlType) {
				if(command!=""){ command+=",";}
				command+="JobControlType = "+POut.Int   ((int)jobControl.JobControlType)+"";
			}
			if(jobControl.ControlData != oldJobControl.ControlData) {
				if(command!=""){ command+=",";}
				command+="ControlData = '"+POut.String(jobControl.ControlData)+"'";
			}
			if(jobControl.XPos != oldJobControl.XPos) {
				if(command!=""){ command+=",";}
				command+="XPos = "+POut.Int(jobControl.XPos)+"";
			}
			if(jobControl.YPos != oldJobControl.YPos) {
				if(command!=""){ command+=",";}
				command+="YPos = "+POut.Int(jobControl.YPos)+"";
			}
			if(jobControl.Width != oldJobControl.Width) {
				if(command!=""){ command+=",";}
				command+="Width = "+POut.Int(jobControl.Width)+"";
			}
			if(jobControl.Height != oldJobControl.Height) {
				if(command!=""){ command+=",";}
				command+="Height = "+POut.Int(jobControl.Height)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE jobcontrol SET "+command
				+" WHERE JobControlNum = "+POut.Long(jobControl.JobControlNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(JobControl,JobControl) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(JobControl jobControl,JobControl oldJobControl) {
			if(jobControl.UserNum != oldJobControl.UserNum) {
				return true;
			}
			if(jobControl.JobControlType != oldJobControl.JobControlType) {
				return true;
			}
			if(jobControl.ControlData != oldJobControl.ControlData) {
				return true;
			}
			if(jobControl.XPos != oldJobControl.XPos) {
				return true;
			}
			if(jobControl.YPos != oldJobControl.YPos) {
				return true;
			}
			if(jobControl.Width != oldJobControl.Width) {
				return true;
			}
			if(jobControl.Height != oldJobControl.Height) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one JobControl from the database.</summary>
		public static void Delete(long jobControlNum){
			string command="DELETE FROM jobcontrol "
				+"WHERE JobControlNum = "+POut.Long(jobControlNum);
			Db.NonQ(command);
		}

	}
}