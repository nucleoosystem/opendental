using Microsoft.CSharp;
//using Microsoft.Vsa;
using System.CodeDom.Compiler;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.ReportingComplex
{
	///<summary></summary>
	public class FormReportComplex : System.Windows.Forms.Form{
		private System.ComponentModel.IContainer components;
		private System.Drawing.Printing.PrintDocument pd2;
		private System.Windows.Forms.PageSetupDialog setupDialog2;
		private OpenDental.UI.ODToolBar ToolBarMain;
		private System.Windows.Forms.ImageList imageListMain;
		private System.Windows.Forms.PrintPreviewControl printPreviewControl2;
		///<summary>The report to display.</summary>
		private ReportComplex _myReport;
		///<summary>The name of the last section printed. It really only keeps track of whether the details section and the reportfooter have finished printing. This variable will be refined when groups are implemented.</summary>
		private AreaSectionType _lastSectionPrinted;
		private int _rowsPrinted;
		private int _totalPages;
		private int _pagesPrinted;
		private int _heightRemaining=0;
    private bool _isWrappingText;

		///<summary></summary>
		public FormReportComplex(ReportComplex myReport){
			InitializeComponent();// Required for Windows Form Designer support
			_myReport=myReport;
		}

		/// <summary>Clean up any resources being used.</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReportComplex));
      this.pd2 = new System.Drawing.Printing.PrintDocument();
      this.setupDialog2 = new System.Windows.Forms.PageSetupDialog();
      this.imageListMain = new System.Windows.Forms.ImageList(this.components);
      this.printPreviewControl2 = new System.Windows.Forms.PrintPreviewControl();
      this.ToolBarMain = new OpenDental.UI.ODToolBar();
      this.SuspendLayout();
      // 
      // imageListMain
      // 
      this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
      this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
      this.imageListMain.Images.SetKeyName(0, "");
      this.imageListMain.Images.SetKeyName(1, "");
      this.imageListMain.Images.SetKeyName(2, "");
      this.imageListMain.Images.SetKeyName(3, "");
      this.imageListMain.Images.SetKeyName(4, "butZoomIn.gif");
      this.imageListMain.Images.SetKeyName(5, "butZoomOut.gif");
      // 
      // printPreviewControl2
      // 
      this.printPreviewControl2.AutoZoom = false;
      this.printPreviewControl2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.printPreviewControl2.Location = new System.Drawing.Point(0, 0);
      this.printPreviewControl2.Name = "printPreviewControl2";
      this.printPreviewControl2.Size = new System.Drawing.Size(831, 570);
      this.printPreviewControl2.TabIndex = 6;
      // 
      // ToolBarMain
      // 
      this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
      this.ToolBarMain.ImageList = this.imageListMain;
      this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
      this.ToolBarMain.Name = "ToolBarMain";
      this.ToolBarMain.Size = new System.Drawing.Size(831, 25);
      this.ToolBarMain.TabIndex = 5;
      this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
      // 
      // FormReportComplex
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(831, 570);
      this.Controls.Add(this.ToolBarMain);
      this.Controls.Add(this.printPreviewControl2);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FormReportComplex";
      this.ShowInTaskbar = false;
      this.Text = "Report";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Load += new System.EventHandler(this.FormReport_Load);
      this.Layout += new System.Windows.Forms.LayoutEventHandler(this.FormReport_Layout);
      this.ResumeLayout(false);

		}
		#endregion

		private void FormReport_Load(object sender, System.EventArgs e) {
      RefreshWindow();
      _isWrappingText=PrefC.GetBool(PrefName.ReportsWrapColumns);
		}

    ///<summary>Used to refresh the print window when something changes.</summary>
    public void RefreshWindow() {
      LayoutToolBar();
			ResetPd2();
			SetDefaultZoom();
			printPreviewControl2.Document=pd2;
    }

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar(){
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print"),0,"","Print"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",1,Lan.g(this,"Go Back One Page"),"Back"));
			ODToolBarButton button=new ODToolBarButton("",-1,"","PageNum");
			button.Style=ODToolBarButtonStyle.Label;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton("",2,Lan.g(this,"Go Forward One Page"),"Fwd"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",4,"","ZoomIn"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",5,"","ZoomOut"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("100",-1,"","ZoomReset"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
      ODToolBarButton butWrapText=new ODToolBarButton(Lan.g(this,"Wrap Text"),-1,Lan.g(this,"Wrap Text In Columns"),"WrapText");
      butWrapText.Style=ODToolBarButtonStyle.ToggleButton;
      butWrapText.Pushed=_isWrappingText;
      ToolBarMain.Buttons.Add(butWrapText);
      ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Export"),3,"","Export"));
      ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,Lan.g(this,"Close This Window"),"Close"));
			//ToolBarMain.Invalidate();
		}

		///<summary>Sets the default zoom factor based on the reports orientation.</summary>
		private void SetDefaultZoom() {
			if(_myReport.IsLandscape) {
				printPreviewControl2.Zoom=((double)printPreviewControl2.ClientSize.Height
					/(double)pd2.DefaultPageSettings.PaperSize.Width);
			}
			else {
				printPreviewControl2.Zoom=((double)printPreviewControl2.ClientSize.Height
					/(double)pd2.DefaultPageSettings.PaperSize.Height);
			}
		}

		private void FormReport_Layout(object sender, System.Windows.Forms.LayoutEventArgs e) {
			printPreviewControl2.Location=new System.Drawing.Point(0,ToolBarMain.Height);
			printPreviewControl2.Height=ClientSize.Height-ToolBarMain.Height;
			printPreviewControl2.Width=ClientSize.Width;	
		}
		
		private void ResetPd2(){
			pd2=new PrintDocument();
			pd2.PrintPage += new PrintPageEventHandler(this.pd2_PrintPage);
			_lastSectionPrinted=AreaSectionType.None;
			_rowsPrinted=0;
			_pagesPrinted=0;
			if(_myReport.IsLandscape){
				pd2.DefaultPageSettings.Landscape=true;
			}
			pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			pd2.OriginAtMargins=true;//the actual margins are taken into consideration in the printpage event, and if the user specifies 0,0 for margins, then the report will reliably print on a preprinted form. Origin is ALWAYS the corner of the paper.
			if(pd2.DefaultPageSettings.PrintableArea.Height==0) {
				pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			foreach(ReportObject reportObject in _myReport.ReportObjects) {
				if(reportObject.ObjectType==ReportObjectType.QueryObject) {
					QueryObject queryObject=(QueryObject)reportObject;
          queryObject.CalculateRowHeights(_isWrappingText);
					if(queryObject.IsPrinted==true) {
						queryObject.IsPrinted=false;
					}
				}
			}
		}

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			//MessageBox.Show(e.Button.Tag.ToString());
			switch(e.Button.Tag.ToString()){
				case "Print":
					OnPrint_Click();
					break;
				case "Back":
					OnBack_Click();
					break;
				case "Fwd":
					OnFwd_Click();
					break;
				case "ZoomIn":
					OnZoomIn_Click();
					break;
				case "ZoomOut":
					OnZoomOut_Click();
					break;
				case "ZoomReset":
					OnZoomReset_Click();
					break;
				case "Export":
					OnExport_Click();
					break;
        case "WrapText":
					OnWrapText_Click();
					break;
				case "Close":
					OnClose_Click();
					break;
				}
		}

		///<summary></summary>
		private void PrintReport(){
			try{
				if(PrinterL.SetPrinter(pd2,PrintSituation.Default,0,"Report printed "+_myReport.ReportName)){
					pd2.Print();
				}
			}
			catch{
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		///<summary>raised for each page to be printed.</summary>
		private void pd2_PrintPage(object sender, PrintPageEventArgs ev){
			//Note that the locations of the reportObjects are not absolute.  They depend entirely upon the margins.  When the report is initially created, it is pushed up against the upper and the left.
			Graphics grfx=ev.Graphics;
			//xPos and yPos represent the upper left of current section after margins are accounted for.
			//All reportObjects are then placed relative to this origin.
			Margins currentMargins=null;
			Size paperSize;
			if(_myReport.IsLandscape) {
				paperSize=new Size(1100,850);
			}
			else {
				paperSize=new Size(850,1100);
			}
			if(_myReport.IsLandscape) {
				currentMargins=new Margins(50,0,30,30);
			}
			else {
				currentMargins=new Margins(30,0,50,50);
			}
			int xPos=currentMargins.Left;
			int yPos=currentMargins.Top;
			int printableHeight=paperSize.Height-currentMargins.Top-currentMargins.Bottom;
			int yLimit=paperSize.Height-currentMargins.Bottom;//the largest yPos allowed
			//Now calculate and layout each section in sequence.
			Section section;
			//Technically the ReportFooter should only be subtracted from the printableHeight of the last page, but we have no way to know how many pages
			//the report will end up taking so we will subtract it from the printable height of all pages.
			//Used to determine the max height of a single grid cell.
			int maxGridCellHeight=printableHeight-_myReport.GetSectionHeight(AreaSectionType.PageHeader)
				-_myReport.GetSectionHeight(AreaSectionType.GroupFooter)-_myReport.GetSectionHeight(AreaSectionType.GroupTitle)
				-_myReport.GetSectionHeight(AreaSectionType.GroupHeader)-_myReport.GetSectionHeight(AreaSectionType.ReportFooter);
			if(_pagesPrinted==0) {
				maxGridCellHeight-=_myReport.GetSectionHeight(AreaSectionType.ReportHeader);
			}
			foreach(ReportObject reportObject in _myReport.ReportObjects) {
				if(reportObject.ObjectType!=ReportObjectType.QueryObject) {
					continue;
				}
				QueryObject queryObject=(QueryObject)reportObject;
				for(int i=0;i<queryObject.RowHeightValues.Count;i++) {
					queryObject.RowHeightValues[i]=Math.Min(queryObject.RowHeightValues[i],maxGridCellHeight);
				}
				foreach(ReportObject rObject in queryObject.ReportObjects) {
					if(rObject.SectionType!=AreaSectionType.Detail && rObject.SectionType!=AreaSectionType.GroupFooter) {
						rObject.ContentAlignment=ContentAlignment.TopCenter;
						continue;
					}
					if(rObject.ObjectType==ReportObjectType.FieldObject && rObject.FieldValueType==FieldValueType.Number) {
						rObject.ContentAlignment=ContentAlignment.TopRight;
					}
				}
			}
			while(true){//will break out if no more room on page
				//if no sections have been printed yet, print a report header.
				if(_lastSectionPrinted==AreaSectionType.None) {
					if(_myReport.Sections.Contains(AreaSectionType.ReportHeader)) {
						section=_myReport.Sections[AreaSectionType.ReportHeader];
						PrintSection(grfx,section,xPos,yPos);
						yPos+=section.Height;
						if(section.Height>printableHeight){//this can happen if the reportHeader takes up the full page
							//if there are no other sections to print
							//this will keep the second page from printing:
							_lastSectionPrinted=AreaSectionType.ReportFooter;
							break;
						}
					}
					else{//no report header
						//it will still be marked as printed on the next line
					}
					_lastSectionPrinted=AreaSectionType.ReportHeader;
				}
				//If the size of pageheader+one detail+pagefooter is taller than page, then we might later display an error. But for now, they will all still be laid out, and whatever goes off the bottom edge will just not show.  This will not be an issue for normal reports:
				if(_myReport.GetSectionHeight(AreaSectionType.PageHeader)
					+_myReport.GetSectionHeight(AreaSectionType.Query)
					+_myReport.GetSectionHeight(AreaSectionType.PageFooter)
					>printableHeight){
					//nothing for now.
				}
				//If this is first page and not enough room to print reportheader+pageheader+detail+pagefooter.
				if(_pagesPrinted==0
					&& _myReport.GetSectionHeight(AreaSectionType.ReportHeader)
					+_myReport.GetSectionHeight(AreaSectionType.PageHeader)
					+_myReport.GetSectionHeight(AreaSectionType.Query)
					+_myReport.GetSectionHeight(AreaSectionType.PageFooter)
					>printableHeight)
				{
					break;
				}
				//always print a page header if it exists
				if(_myReport.Sections.Contains(AreaSectionType.PageHeader)) {
					section=_myReport.Sections[AreaSectionType.PageHeader];
					PrintSection(grfx,section,xPos,yPos);
					yPos+=section.Height;
				}
				_heightRemaining=yLimit-yPos-_myReport.GetSectionHeight(AreaSectionType.PageFooter);
				section=_myReport.Sections[AreaSectionType.Query];
				PrintQuerySection(grfx,section,xPos,yPos);
				yPos+=section.Height;
				bool isRoomForReportFooter=true;
				if(_heightRemaining-_myReport.GetSectionHeight(AreaSectionType.ReportFooter)<=0) {
					isRoomForReportFooter=false;
				}
				//print the reportfooter section if there is room
				if(isRoomForReportFooter){
					if(_myReport.Sections.Contains(AreaSectionType.ReportFooter)) {
						section=_myReport.Sections[AreaSectionType.ReportFooter];
						PrintSection(grfx,section,xPos,yPos);
						yPos+=section.Height;
					}
					//mark the reportfooter as printed. This will prevent another loop.
					_lastSectionPrinted=AreaSectionType.ReportFooter;
				}
				//print the pagefooter
				if(_myReport.Sections.Contains(AreaSectionType.PageFooter)) {
					section=_myReport.Sections[AreaSectionType.PageFooter];
					yPos=yLimit-section.Height;
					PrintSection(grfx,section,xPos,yPos);
					yPos+=section.Height;
				}
				break;
			}//while			
			_pagesPrinted++;
			//if the reportfooter has been printed, then there are no more pages.
			if(_lastSectionPrinted==AreaSectionType.ReportFooter) {
				ev.HasMorePages=false;
				_totalPages=_pagesPrinted;
				ToolBarMain.Buttons["PageNum"].Text="1 / "+_totalPages.ToString();
				ToolBarMain.Invalidate();
				//labelTotPages.Text="1 / "+totalPages.ToString();
			}
			else{
				ev.HasMorePages=true;
			}
		}

		///<summary>Prints one section other than details at the specified x and y position on the page.  The math to decide whether it will fit on the current page is done ahead of time.</summary>
		private void PrintSection(Graphics g,Section section,int xPos,int yPos){
			ReportObject textObject;
			ReportObject fieldObject;
			ReportObject lineObject;
			ReportObject boxObject;
			StringFormat strFormat;//used each time text is drawn to handle alignment issues
			string displayText="";//The formatted text to print
			foreach(ReportObject reportObject in _myReport.ReportObjects){
				if(reportObject.SectionType!=section.SectionType){
					continue;
				}
				if(reportObject.ObjectType==ReportObjectType.TextObject){
					textObject=reportObject;
					Font newFont=textObject.Font;
					strFormat=ReportObject.GetStringFormatAlignment(textObject.ContentAlignment);
					if(section.SectionType==AreaSectionType.ReportFooter) {
						if(textObject.Name=="ReportSummaryText") {
							xPos+=_myReport.ReportObjects["ReportSummaryLabel"].Size.Width;
							if(textObject.IsUnderlined) {
								newFont=new Font(textObject.Font.FontFamily,textObject.Font.Size,FontStyle.Bold|FontStyle.Underline);
							}
							else {
								newFont=new Font(textObject.Font.FontFamily,textObject.Font.Size,FontStyle.Bold);
							}
							SizeF size=g.MeasureString(textObject.StaticText,newFont);
							textObject.Size=new Size((int)size.Width+1,(int)size.Height+1);
						}
						strFormat=ReportObject.GetStringFormatAlignment(textObject.ContentAlignment);
						RectangleF layoutRect=new RectangleF(xPos+textObject.Location.X+textObject.OffSetX
							,yPos+textObject.Location.Y+textObject.OffSetY
							,textObject.Size.Width,textObject.Size.Height);
						if(textObject.IsUnderlined) {
							g.DrawString(textObject.StaticText,new Font(textObject.Font.FontFamily,textObject.Font.Size,textObject.Font.Style|FontStyle.Underline),Brushes.Black,layoutRect,strFormat);
						}
						else {
							g.DrawString(textObject.StaticText,newFont,Brushes.Black,layoutRect,strFormat);
							//g.DrawLine(new Pen(textObject.ForeColor),xPos+textObject.Location.X+textObject.OffSetX,yPos+textObject.Location.Y+textObject.OffSetY+textObject.Size.Height,xPos+textObject.Location.X+textObject.Size.Width,yPos+textObject.Location.Y+textObject.Size.Height);
						}
					}
					else {
						strFormat=ReportObject.GetStringFormatAlignment(textObject.ContentAlignment);
						RectangleF layoutRect=new RectangleF(xPos+textObject.Location.X
							,yPos+textObject.Location.Y
							,textObject.Size.Width,textObject.Size.Height);
						if(textObject.IsUnderlined) {
							g.DrawString(textObject.StaticText,new Font(textObject.Font.FontFamily,textObject.Font.Size,textObject.Font.Style|FontStyle.Underline),Brushes.Black,layoutRect,strFormat);
						}
						else {
							g.DrawString(textObject.StaticText,textObject.Font,Brushes.Black,layoutRect,strFormat);
						}
					}
				}
				else if(reportObject.ObjectType==ReportObjectType.FieldObject){
					fieldObject=reportObject;
					strFormat=ReportObject.GetStringFormatAlignment(fieldObject.ContentAlignment);
					RectangleF layoutRect=new RectangleF(xPos+fieldObject.Location.X
						,yPos+fieldObject.Location.Y
						,fieldObject.Size.Width,fieldObject.Size.Height);
					displayText="";
					if(fieldObject.FieldDefKind==FieldDefKind.SummaryField) {
						//displayText=fieldObject.GetSummaryValue
						//	(_myReport.ReportTables,_myReport.DataFields.IndexOf
						//	(fieldObject.SummarizedField))
						//	.ToString(fieldObject.FormatString);
					}
					else if(fieldObject.FieldDefKind==FieldDefKind.SpecialField){
						if(fieldObject.SpecialFieldType==SpecialFieldType.PageNofM){//not functional yet
							//displayText=Lan.g(this,"Page")+" "+(pagesPrinted+1).ToString()
							//	+Lan.g(
						}
						else if(fieldObject.SpecialFieldType==SpecialFieldType.PageNumber){
							displayText=Lan.g(this,"Page")+" "+(_pagesPrinted+1).ToString();
						}
					}
					g.DrawString(displayText,fieldObject.Font,Brushes.Black,layoutRect,strFormat);
				}
				else if(reportObject.ObjectType==ReportObjectType.BoxObject) {
					boxObject=reportObject;
					int x1=xPos+boxObject.OffSetX;
					int x2=xPos-boxObject.OffSetX;
					int y1=yPos+boxObject.OffSetY;
					int y2=yPos-boxObject.OffSetY;
					int maxHorizontalLength=1100;
					if(!_myReport.IsLandscape) {
						maxHorizontalLength=850;
					}
					x2+=maxHorizontalLength;
					y2+=_myReport.GetSectionHeight(boxObject.SectionType);
					g.DrawRectangle(new Pen(boxObject.ForeColor,boxObject.FloatLineThickness),x1,y1,x2-x1,y2-y1);
				}
				else if(reportObject.ObjectType==ReportObjectType.LineObject) {
					lineObject=reportObject;
					int length;
					int x=lineObject.OffSetX;
					int y=yPos+lineObject.OffSetY;
					int maxHorizontalLength=1100;
					if(!_myReport.IsLandscape) {
						maxHorizontalLength=850;
					}
					if(lineObject.LineOrientation==LineOrientation.Horizontal) {
						length=maxHorizontalLength*lineObject.IntLinePercent/100;
						if(lineObject.LinePosition==LinePosition.South) {
							y+=_myReport.GetSectionHeight(lineObject.SectionType);
						}
						else if(lineObject.LinePosition==LinePosition.North) {
							//Do Nothing Here
						}
						else if(lineObject.LinePosition==LinePosition.Center) {
							y+=(_myReport.GetSectionHeight(lineObject.SectionType)/2);
						}
						else {
							continue;
						}
						x+=(maxHorizontalLength/2)-(length/2);
						g.DrawLine(new Pen(reportObject.ForeColor,reportObject.FloatLineThickness),x,y,x+length,y);
					}
					else if(lineObject.LineOrientation==LineOrientation.Vertical) {
						length=_myReport.GetSectionHeight(lineObject.SectionType)*lineObject.IntLinePercent/100;
						if(lineObject.LinePosition==LinePosition.West) {
							//Do Nothing Here
						}
						else if(lineObject.LinePosition==LinePosition.East) {
							x+=maxHorizontalLength;
						}
						else if(lineObject.LinePosition==LinePosition.Center) {
							x+=maxHorizontalLength/2;
						}
						else {
							continue;
						}
						y+=(_myReport.GetSectionHeight(lineObject.SectionType)/2)-(length/2);
						g.DrawLine(new Pen(reportObject.ForeColor,reportObject.FloatLineThickness),x,y,x,y+length);
					}
				}
			}
		}

		///<summary>Prints some rows of the details section at the specified x and y position on the page.  The math to decide how many rows to print is done ahead of time.  The number of rows printed so far is kept global so that it can be used in calculating the layout of this section.</summary>
		private void PrintQuerySection(Graphics g,Section section,int xPos,int yPos) {
			section.Height=0;
			ReportObject textObject;
			ReportObject lineObject;
			ReportObject boxObject;
			QueryObject queryObject;
			StringFormat strFormat;//used each time text is drawn to handle alignment issues
			#region Lines And Boxes
			foreach(ReportObject reportObject in _myReport.ReportObjects) {
				if(reportObject.SectionType!=section.SectionType) {
					//skip any reportObjects that are not in this section
					continue;
				}

				if(reportObject.ObjectType==ReportObjectType.BoxObject) {
					boxObject=reportObject;
					int x1=xPos+boxObject.OffSetX;
					int x2=xPos-boxObject.OffSetX;
					int y1=yPos+boxObject.OffSetY;
					int y2=yPos-boxObject.OffSetY;
					int maxHorizontalLength=1100;
					if(!_myReport.IsLandscape) {
						maxHorizontalLength=850;
					}
					x2+=maxHorizontalLength-xPos;
					y2+=_heightRemaining*_myReport.GetSectionHeight(boxObject.SectionType);
					g.DrawRectangle(new Pen(boxObject.ForeColor,boxObject.FloatLineThickness),x1,y1,x2-x1,y2-y1);
				}
				else if(reportObject.ObjectType==ReportObjectType.LineObject) {
					lineObject=reportObject;
					int length;
					int x=lineObject.OffSetX;
					int y=yPos+lineObject.OffSetY;
					int maxHorizontalLength=1100;
					if(!_myReport.IsLandscape) {
						maxHorizontalLength=850;
					}
					if(lineObject.LineOrientation==LineOrientation.Horizontal) {
						length=maxHorizontalLength*lineObject.IntLinePercent/100;
						if(lineObject.LinePosition==LinePosition.South) {
							y+=_myReport.GetSectionHeight(lineObject.SectionType);
						}
						else if(lineObject.LinePosition==LinePosition.North) {
							//Do Nothing Here
						}
						else if(lineObject.LinePosition==LinePosition.Center) {
							y+=(_myReport.GetSectionHeight(lineObject.SectionType)/2);
						}
						else {
							continue;
						}
						x+=(maxHorizontalLength/2)-(length/2);
						g.DrawLine(new Pen(reportObject.ForeColor,reportObject.FloatLineThickness),x,y,x+length,y);
					}
					else if(lineObject.LineOrientation==LineOrientation.Vertical) {
						length=_myReport.GetSectionHeight(lineObject.SectionType)*lineObject.IntLinePercent/100;
						if(lineObject.LinePosition==LinePosition.West) {
							//Do Nothing Here
						}
						else if(lineObject.LinePosition==LinePosition.East) {
							x=maxHorizontalLength;
						}
						else if(lineObject.LinePosition==LinePosition.Center) {
							x=maxHorizontalLength/2;
						}
						else {
							continue;
						}
						y=y+(_myReport.GetSectionHeight(lineObject.SectionType)/2)-(length/2);
						g.DrawLine(new Pen(reportObject.ForeColor,reportObject.FloatLineThickness),x,y,x,y+length);
					}
					else {
						//Do nothing since it has already been done for each row.
					}
				}
			}
			#endregion
			foreach(ReportObject reportObject in _myReport.ReportObjects) {
				if(reportObject.SectionType!=section.SectionType) {
					//skip any reportObjects that are not in this section
					continue;
				}
				if(reportObject.ObjectType==ReportObjectType.TextObject) {
					//not typical to print textobject in details section, but allowed
					textObject=reportObject;
					strFormat=ReportObject.GetStringFormatAlignment(textObject.ContentAlignment);
					RectangleF layoutRect=new RectangleF(xPos+textObject.Location.X
						,yPos+textObject.Location.Y
						,textObject.Size.Width,textObject.Size.Height);
					g.DrawString(textObject.StaticText,textObject.Font
						,new SolidBrush(textObject.ForeColor),layoutRect,strFormat);
					if(textObject.IsUnderlined) {
						g.DrawLine(new Pen(textObject.ForeColor),xPos+textObject.Location.X,yPos+textObject.Location.Y+textObject.Size.Height,xPos+textObject.Location.X+textObject.Size.Width,yPos+textObject.Location.Y+textObject.Size.Height);
					}
				}
				else if(reportObject.ObjectType==ReportObjectType.QueryObject) {
					queryObject=(QueryObject)reportObject;
					if(queryObject.IsPrinted==true) {
						continue;
					}
					if(queryObject.IsCentered) {
						if(_myReport.IsLandscape) {
							xPos=1100/2-(queryObject.QueryWidth/2);
						}
						else {
							xPos=850/2-(queryObject.QueryWidth/2);
						}
					}
					if(_heightRemaining>0) {
						PrintQueryObjectSection(queryObject,g,queryObject.Sections[AreaSectionType.GroupTitle],xPos,yPos);
						yPos+=queryObject.Sections[AreaSectionType.GroupTitle].Height;
						section.Height+=queryObject.Sections[AreaSectionType.GroupTitle].Height;
					}
					if(_heightRemaining>0) {
						PrintQueryObjectSection(queryObject,g,queryObject.Sections[AreaSectionType.GroupHeader],xPos,yPos);
						yPos+=queryObject.Sections[AreaSectionType.GroupHeader].Height;
						section.Height+=queryObject.Sections[AreaSectionType.GroupHeader].Height;
					}
					if(_heightRemaining>0) {
						PrintQueryObjectSection(queryObject,g,queryObject.Sections[AreaSectionType.Detail],xPos,yPos);
						yPos+=queryObject.Sections[AreaSectionType.Detail].Height;
						section.Height+=queryObject.Sections[AreaSectionType.Detail].Height;
					}
					if(_heightRemaining>0) {
						PrintQueryObjectSection(queryObject,g,queryObject.Sections[AreaSectionType.GroupFooter],xPos,yPos);
						yPos+=queryObject.Sections[AreaSectionType.GroupFooter].Height;
						section.Height+=queryObject.Sections[AreaSectionType.GroupFooter].Height;
					}
					if(_heightRemaining<=0) {
						return;
					}
				}
			}
		}

		///<summary>Prints sections inside a QueryObject</summary>
		private void PrintQueryObjectSection(QueryObject queryObj,Graphics g,Section section,int xPos,int yPos) {
			section.Height=0;
			ReportObject textObject;
			ReportObject fieldObject;
			ReportObject lineObject;
			ReportObject boxObject;
			string rawText="";//the raw text for a given field as taken from the database
			string displayText="";//The formatted text to print
			string prevDisplayText="";//The formatted text of the previous row. Used to test suppress dupl.	
			StringFormat strFormat;//used each time text is drawn to handle alignment issues
			int yPosAdd=0;
			if(queryObj.SuppressIfDuplicate
				&& section.SectionType==AreaSectionType.GroupTitle && _rowsPrinted>0) 
			{
				return;//Only print the group title for each query object once.
			}
			//loop through each row in the table and make sure that the row can fit.  If it can fit, print it.  Otherwise go to next page.
			for(int i=_rowsPrinted;i<queryObj.ReportTable.Rows.Count;i++) {
				//Figure out the current row height
				if(section.SectionType==AreaSectionType.Detail && queryObj.RowHeightValues[i]>_heightRemaining) {
					_heightRemaining=0;
					return;
				}
				//Find the Group Header height to see if printing at least one row is possible.
				if(section.SectionType==AreaSectionType.GroupTitle) {
					int titleHeight=0;
					int headerHeight=0;
					foreach(ReportObject reportObject in queryObj.ReportObjects) {
						if(reportObject.SectionType==AreaSectionType.GroupTitle) {
							titleHeight+=reportObject.Size.Height;
						}
						else if(reportObject.SectionType==AreaSectionType.GroupHeader && reportObject.Size.Height>headerHeight) {
							headerHeight=reportObject.Size.Height;
						}
					}
					//This is a new table and we want to know if we can print the first row
					if(titleHeight+headerHeight+queryObj.RowHeightValues[0]>_heightRemaining) {
						_heightRemaining=0;
						return;
					}
				}
				//Find the Group Footer height to see if printing the last row should happen on another page.
				if(section.SectionType==AreaSectionType.Detail && _rowsPrinted==queryObj.ReportTable.Rows.Count-1) {
					int groupSummaryLabelHeight=0;
					int tallestTotalSummaryHeight=0;
					foreach(ReportObject reportObject in queryObj.ReportObjects) {
						if(reportObject.SectionType==AreaSectionType.GroupFooter 
							&& (!reportObject.Name.Contains("GroupSummaryLabel") || !reportObject.Name.Contains("GroupSummaryText"))
							&& tallestTotalSummaryHeight<reportObject.Size.Height+reportObject.OffSetY)
						{
							tallestTotalSummaryHeight=reportObject.Size.Height+reportObject.OffSetY;
						}
						//Find the height of the group footer using GroupSummaryLabel because GroupSummaryText has not been filled yet.
						if(reportObject.SectionType==AreaSectionType.GroupFooter && reportObject.Name.Contains("GroupSummaryLabel")) {
							groupSummaryLabelHeight+=reportObject.Size.Height+reportObject.OffSetY;
						}
						//The GroupSummaryText hasn't been filled yet so we use GroupSummaryLabel again
						//If it is North or South then we need to add its height a second time because the GroupSummaryLabel is located above or below the text.
						if(reportObject.SectionType==AreaSectionType.GroupFooter && reportObject.Name.Contains("GroupSummaryLabel") 
							&& (reportObject.SummaryOrientation==SummaryOrientation.North || reportObject.SummaryOrientation==SummaryOrientation.South)) 
						{
							groupSummaryLabelHeight+=reportObject.Size.Height;
						}
					}
					int groupFooterHeight=groupSummaryLabelHeight+tallestTotalSummaryHeight;
					//See if we can print the Group Footer and the Last row
					if(groupFooterHeight+queryObj.RowHeightValues[queryObj.ReportTable.Rows.Count-1]>_heightRemaining) {
						_heightRemaining=0;
						return;
					}
				}
				int greatestObjectHeight=0;
				int groupTitleHeight=0;
				//Now figure out if anything in the header, footer, or title sections can still fit on the page
				foreach(ReportObject reportObject in queryObj.ReportObjects) {
					if(reportObject.SectionType!=section.SectionType) {
						continue;
					}
					if(reportObject.Size.Height>_heightRemaining) {
						_heightRemaining=0;
						return;
					}
					if(reportObject.SectionType==AreaSectionType.GroupFooter && reportObject.Name.Contains("GroupSummary")) {
						if(!queryObj.IsLastSplit) {
							continue;
						}
						if(reportObject.Name.Contains("GroupSummaryLabel")) {
							yPos+=reportObject.OffSetY;
						}
						if(reportObject.Name.Contains("GroupSummaryText")) {
							if(reportObject.SummaryOperation==SummaryOperation.Sum) {
								reportObject.StaticText=GetGroupSummaryValue(reportObject.DataField,reportObject.SummaryGroups,reportObject.SummaryOperation).ToString("c");
							}
							else if(reportObject.SummaryOperation==SummaryOperation.Count) {
								reportObject.StaticText=GetGroupSummaryValue(reportObject.DataField,reportObject.SummaryGroups,reportObject.SummaryOperation).ToString();
							}
							int width=(int)g.MeasureString(reportObject.StaticText,reportObject.Font).Width+2;
							int height=(int)g.MeasureString(reportObject.StaticText,reportObject.Font).Height+2;
							if(width<queryObj.GetObjectByName(reportObject.SummarizedField+"Header").Size.Width) {
								width=queryObj.GetObjectByName(reportObject.SummarizedField+"Header").Size.Width;
							}
							reportObject.Size=new Size(width,height);
						}
					}
					if(section.SectionType==AreaSectionType.GroupTitle && _rowsPrinted>0 && reportObject.Name=="Initial Group Title") {
						continue;
					}
					if(section.SectionType==AreaSectionType.GroupFooter && reportObject.SummaryOrientation==SummaryOrientation.South) {
						ReportObject summaryField=queryObj.GetObjectByName(reportObject.DataField+"Footer");
						yPos+=summaryField.Size.Height;
					}
					if(reportObject.ObjectType==ReportObjectType.TextObject) {
						textObject=reportObject;
						strFormat=ReportObject.GetStringFormatAlignment(textObject.ContentAlignment);
						RectangleF layoutRect=new RectangleF(xPos+textObject.Location.X+textObject.OffSetX
							,yPos+textObject.Location.Y+textObject.OffSetY
							,textObject.Size.Width,textObject.Size.Height);
						if(textObject.IsUnderlined) {
							g.DrawString(textObject.StaticText,new Font(textObject.Font.FontFamily,textObject.Font.Size,textObject.Font.Style|FontStyle.Underline),Brushes.Black,layoutRect,strFormat);
						}
						else {
							g.DrawString(textObject.StaticText,textObject.Font,Brushes.Black,layoutRect,strFormat);
						}
						if(greatestObjectHeight<textObject.Size.Height) {
							greatestObjectHeight=textObject.Size.Height;
						}
						groupTitleHeight+=textObject.Size.Height;
						if(section.SectionType==AreaSectionType.GroupTitle) {
							yPos+=textObject.Size.Height;
						}
						if(section.SectionType==AreaSectionType.GroupFooter 
							&& ((reportObject.SummaryOrientation==SummaryOrientation.North || reportObject.SummaryOrientation==SummaryOrientation.South)
								|| (reportObject.Name.Contains("GroupSummaryText")))) 
						{
							yPosAdd+=textObject.Size.Height;
							yPos+=textObject.Size.Height;
						}
					}
					else if(reportObject.ObjectType==ReportObjectType.BoxObject) {
						boxObject=reportObject;
						int x1=xPos+boxObject.OffSetX;
						int x2=xPos-boxObject.OffSetX;
						int y1=yPos+boxObject.OffSetY;
						int y2=yPos-boxObject.OffSetY;
						int maxHorizontalLength=1100;
						if(!_myReport.IsLandscape) {
							maxHorizontalLength=850;
						}
						x2+=maxHorizontalLength;
						y2+=queryObj.GetSectionHeight(boxObject.SectionType);
						g.DrawRectangle(new Pen(boxObject.ForeColor,boxObject.FloatLineThickness),x1,y1,x2-x1,y2-y1);
						if(greatestObjectHeight<boxObject.Size.Height) {
							greatestObjectHeight=boxObject.Size.Height;
						}
						groupTitleHeight+=boxObject.Size.Height;
					}
					else if(reportObject.ObjectType==ReportObjectType.LineObject) {
						lineObject=reportObject;
						int length;
						int x=lineObject.OffSetX;
						int y=yPos+lineObject.OffSetY;
						int maxHorizontalLength=1100;
						if(!_myReport.IsLandscape) {
							maxHorizontalLength=850;
						}
						if(lineObject.LineOrientation==LineOrientation.Horizontal) {
							length=maxHorizontalLength*lineObject.IntLinePercent/100;
							if(lineObject.LinePosition==LinePosition.South) {
								y+=queryObj.GetSectionHeight(lineObject.SectionType);
							}
							else if(lineObject.LinePosition==LinePosition.North) {
								//Do Nothing Here
							}
							else if(lineObject.LinePosition==LinePosition.Center) {
								y+=(queryObj.GetSectionHeight(lineObject.SectionType)/2);
							}
							else {
								continue;
							}
							x+=(maxHorizontalLength/2)-(length/2);
							g.DrawLine(new Pen(reportObject.ForeColor,reportObject.FloatLineThickness),x,y,x+length,y);
						}
						else if(lineObject.LineOrientation==LineOrientation.Vertical) {
							length=queryObj.GetSectionHeight(lineObject.SectionType)*lineObject.IntLinePercent/100;
							if(lineObject.LinePosition==LinePosition.West) {
								//Do Nothing Here
							}
							else if(lineObject.LinePosition==LinePosition.East) {
								x+=maxHorizontalLength;
							}
							else if(lineObject.LinePosition==LinePosition.Center) {
								x+=maxHorizontalLength/2;
							}
							else {
								continue;
							}
							y+=(queryObj.GetSectionHeight(lineObject.SectionType)/2)-(length/2);
							g.DrawLine(new Pen(reportObject.ForeColor,reportObject.FloatLineThickness),x,y,x,y+length);
						}
						if(greatestObjectHeight<lineObject.Size.Height) {
							greatestObjectHeight=lineObject.Size.Height;
						}
						groupTitleHeight+=lineObject.Size.Height;
					}
					else if(reportObject.ObjectType==ReportObjectType.FieldObject) {
						fieldObject=reportObject;
						RectangleF layoutRect;
						strFormat=ReportObject.GetStringFormatAlignment(fieldObject.ContentAlignment);
						if(fieldObject.FieldDefKind==FieldDefKind.DataTableField) {
							layoutRect=new RectangleF(xPos+fieldObject.Location.X,yPos+fieldObject.Location.Y,fieldObject.Size.Width,queryObj.RowHeightValues[i]);
							if(_myReport.HasGridLines()) {
								g.DrawRectangle(new Pen(Brushes.LightGray),Rectangle.Round(layoutRect));
							}
							rawText=queryObj.ReportTable.Rows
								[i][queryObj.ArrDataFields.IndexOf(fieldObject.DataField)].ToString();
							displayText=rawText;
							List<string> listString=GetDisplayString(displayText,prevDisplayText,fieldObject,i,queryObj);
							displayText=listString[0];
							prevDisplayText=listString[1];
							//suppress if duplicate:
							if(i>0 && fieldObject.SuppressIfDuplicate && displayText==prevDisplayText) {
								displayText="";
							}
						}
						else {
							layoutRect=new RectangleF(xPos+fieldObject.Location.X,yPos+fieldObject.Location.Y,fieldObject.Size.Width,fieldObject.Size.Height);
							displayText=fieldObject.GetSummaryValue(queryObj.ReportTable,queryObj.ArrDataFields.IndexOf(fieldObject.SummarizedField)).ToString(fieldObject.StringFormat);
						}
						g.DrawString(displayText,fieldObject.Font
						,new SolidBrush(fieldObject.ForeColor),new RectangleF(layoutRect.X+1,layoutRect.Y+1,layoutRect.Width,layoutRect.Height-1),strFormat);
						yPosAdd=(int)layoutRect.Height;
					}
				}
				if(section.SectionType==AreaSectionType.GroupFooter) {
					yPosAdd+=20;//Added to give a buffer between split tables.
					section.Height+=yPosAdd;
					_heightRemaining-=section.Height;
					yPos+=yPosAdd;
					break;
				}
				else if(section.SectionType==AreaSectionType.GroupTitle) {
					section.Height+=groupTitleHeight;
					_heightRemaining-=section.Height;
					break;
				}
				else if(section.SectionType==AreaSectionType.GroupHeader) {
					section.Height=greatestObjectHeight;
					_heightRemaining-=section.Height;
					break;
				}
				if(section.SectionType==AreaSectionType.Detail) {
					_rowsPrinted++;
					yPos+=yPosAdd;
					_heightRemaining-=yPosAdd;
					section.Height+=yPosAdd;
				}
			}
			if(_rowsPrinted==queryObj.ReportTable.Rows.Count) {
				_rowsPrinted=0;
				queryObj.IsPrinted=true;
			}
		}

		private double GetGroupSummaryValue(string columnName,List<int> summaryGroups,SummaryOperation operation) {
			double retVal=0;
			for(int i=0;i<_myReport.ReportObjects.Count;i++) {
				if(_myReport.ReportObjects[i].ObjectType!=ReportObjectType.QueryObject) {
					continue;
				}
				QueryObject queryObj=(QueryObject)_myReport.ReportObjects[i];
				if(!summaryGroups.Contains(queryObj.QueryGroupValue)) {
					continue;
				}
				for(int j=0;j<queryObj.ReportTable.Rows.Count;j++) {
					if(operation==SummaryOperation.Sum) {
						//This could be enhanced in the future to only sum up the cells that match the column name within the current query group.
						//Right now, if multiple query groups share the same column name that is being summed, the total will include both sets.
						if(queryObj.IsNegativeSummary) {
							retVal-=PIn.Double(queryObj.ReportTable.Rows[j][queryObj.ReportTable.Columns.IndexOf(columnName)].ToString());
						}
						else {
							retVal+=PIn.Double(queryObj.ReportTable.Rows[j][queryObj.ReportTable.Columns.IndexOf(columnName)].ToString());
						}
					}
					else if(operation==SummaryOperation.Count) {
						retVal++;
					}
				}
			}
			return retVal;
		}

		private List<string> GetDisplayString(string rawText,string prevDisplayText,ReportObject reportObject,int i,QueryObject queryObj) {
			return GetDisplayString(rawText,prevDisplayText,reportObject,i,queryObj,false);
		}

		private List<string> GetDisplayString(string rawText,string prevDisplayText,ReportObject reportObject,int i,QueryObject queryObj,bool isExport) {
			string displayText="";
			List<string> retVals=new List<string>();
			DataTable dt=queryObj.ReportTable;
			//For exporting, we need to use the ExportTable which is the data that is visible to the user.  Using ReportTable would show raw query data (potentially different than what the user sees).
			if(isExport) {
				dt=queryObj.ExportTable;
			}
			if(reportObject.FieldValueType==FieldValueType.Age) {
				displayText=Patients.AgeToString(Patients.DateToAge(PIn.Date(rawText)));//(fieldObject.FormatString);
			}
			else if(reportObject.FieldValueType==FieldValueType.Boolean) {
				if(PIn.Bool(dt.Rows[i][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString())) {
					displayText="X";
				}
				else {
					displayText="";
				}
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.Bool(dt.Rows[i-1][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString();
				}
			}
			else if(reportObject.FieldValueType==FieldValueType.Date) {
				displayText=PIn.DateT(dt.Rows[i][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.DateT(dt.Rows[i-1][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				}
			}
			else if(reportObject.FieldValueType==FieldValueType.Integer) {
				displayText=PIn.Long(dt.Rows[i][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.Long(dt.Rows[i-1][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				}
			}
			else if(reportObject.FieldValueType==FieldValueType.Number) {
				displayText=PIn.Double(dt.Rows[i][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.Double(dt.Rows[i-1][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				}
			}
			else if(reportObject.FieldValueType==FieldValueType.String) {
				displayText=rawText;
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=dt.Rows[i-1][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString();
				}
			}
			retVals.Add(displayText);
			retVals.Add(prevDisplayText);
			return retVals;
		}

		private void butSetup_Click(object sender, System.EventArgs e) {
			setupDialog2.AllowMargins=true;
			setupDialog2.AllowOrientation=true;
			setupDialog2.AllowPaper=false;
			setupDialog2.AllowPrinter=false;
			setupDialog2.Document=pd2;
			setupDialog2.ShowDialog();
		}

		private void OnPrint_Click() {
			ResetPd2();
			PrintReport();
		}

		private void OnBack_Click(){
			PrevPage();
		}

		private void OnFwd_Click(){
			NextPage();
		}

		private void OnZoomIn_Click() {
			printPreviewControl2.Zoom=printPreviewControl2.Zoom*2;
		}

		private void OnZoomOut_Click() {
			printPreviewControl2.Zoom=printPreviewControl2.Zoom/2;
		}

		private void OnZoomReset_Click() {
			SetDefaultZoom();
		}

		private void PrevPage() {
			if(printPreviewControl2.StartPage==0) return;
			printPreviewControl2.StartPage--;
			ToolBarMain.Buttons["PageNum"].Text=(printPreviewControl2.StartPage+1).ToString()
				+" / "+_totalPages.ToString();
			ToolBarMain.Invalidate();
		}

		private void NextPage() {
			if(printPreviewControl2.StartPage==_totalPages-1) return;
			printPreviewControl2.StartPage++;
			ToolBarMain.Buttons["PageNum"].Text=(printPreviewControl2.StartPage+1).ToString()
				+" / "+_totalPages.ToString();
			ToolBarMain.Invalidate();
		}

		private void OnExport_Click(){
			SaveFileDialog saveFileDialog2=new SaveFileDialog();
			saveFileDialog2.AddExtension=true;
			//saveFileDialog2.Title=Lan.g(this,"Select Folder to Save File To");
			saveFileDialog2.FileName=_myReport.ReportName+".txt";
			if(!Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
				try {
					Directory.CreateDirectory(PrefC.GetString(PrefName.ExportPath));
					saveFileDialog2.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
				}
				catch {
					//initialDirectory will be blank
				}
			}
			else {
				saveFileDialog2.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			}
			//saveFileDialog2.DefaultExt="txt";
			saveFileDialog2.Filter="Text files(*.txt)|*.txt|Excel Files(*.xls)|*.xls|All files(*.*)|*.*";
			saveFileDialog2.FilterIndex=0;
			if(saveFileDialog2.ShowDialog()!=DialogResult.OK) {
				return;
			}
			try {
				using(StreamWriter sw=new StreamWriter(saveFileDialog2.FileName,false)) {
					String line="";
					foreach(ReportObject reportObject in _myReport.ReportObjects) {
						if(reportObject.ObjectType==ReportObjectType.QueryObject) {
							QueryObject query=(QueryObject)reportObject;
							line=query.GetGroupTitle().StaticText;
							sw.WriteLine(line);
							line="";
							for(int i=0;i<query.ExportTable.Columns.Count;i++) {
								line+=query.ExportTable.Columns[i].Caption;
								if(i<query.ExportTable.Columns.Count-1) {
									line+="\t";
								}
							}
							sw.WriteLine(line);
							string cell;
							for(int i=0;i<query.ExportTable.Rows.Count;i++) {
								line="";
								string displayText="";
								foreach(ReportObject reportObj in query.ReportObjects) {
									if(reportObj.SectionType!=AreaSectionType.Detail) {
										continue;
									}
									string rawText="";
									if(reportObj.ObjectType==ReportObjectType.FieldObject) {
										rawText=query.ExportTable.Rows[i][query.ArrDataFields.IndexOf(reportObj.DataField)].ToString();
										if(String.IsNullOrWhiteSpace(rawText)) {
											line+="\t";
											continue;
										}
										List<string> listString=GetDisplayString(rawText,"",reportObj,i,query,true);
										displayText=listString[0];
									}
									cell=displayText;
									cell=cell.Replace("\r","");
									cell=cell.Replace("\n","");
									cell=cell.Replace("\t","");
									cell=cell.Replace("\"","");
									line+=cell;
									if(query.ArrDataFields.IndexOf(reportObj.DataField)<query.ArrDataFields.Count-1) {
										line+="\t";
									}
								}
								sw.WriteLine(line);
							}
							int columnValue=-1;
							line="";
							foreach(ReportObject reportObjQuery in query.ReportObjects) {
								if(reportObjQuery.SectionType==AreaSectionType.GroupFooter && reportObjQuery.Name.Contains("Footer")) {
									if(columnValue==-1) {
										columnValue=query.ArrDataFields.IndexOf(reportObjQuery.SummarizedField);
										for(int i=0;i<columnValue;i++) {
											line+=" \t";
										}
									}
									line+=reportObjQuery.GetSummaryValue(query.ExportTable,query.ArrDataFields.IndexOf(reportObjQuery.SummarizedField)).ToString(reportObjQuery.StringFormat)+"\t";
								}
							}
							sw.WriteLine(line);
						}
					}
				}//using
			}
			catch {
				MessageBox.Show(Lan.g(this,"File in use by another program.  Close and try again."));
				return;
			}
			MessageBox.Show(Lan.g(this,"File created successfully"));
		}

    private void OnWrapText_Click() {
      _isWrappingText=!_isWrappingText;
			RefreshWindow();
		}

		private void OnClose_Click() {
			this.Close();
		}

		private void button1_Click(object sender,System.EventArgs e) {
			//ScriptEngine.FormulaCode = 
			/*string functionCode=
			@"using System.Windows.Forms;
				using System;
				public class Test{
					public static void Main(){
						MessageBox.Show(""This is a test"");
						Test2 two = new Test2();
						two.Stuff();
					}
				}
				public class Test2{
					public void Stuff(){

					}
				}";
			CodeDomProvider codeProvider=new CSharpCodeProvider();
			ICodeCompiler compiler = codeProvider.CreateCompiler();
			CompilerParameters compilerParams = new CompilerParameters();
			compilerParams.CompilerOptions = "/target:library /optimize";
			compilerParams.GenerateExecutable = false;
			compilerParams.GenerateInMemory = true;
			compilerParams.IncludeDebugInformation = false;
			compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
			compilerParams.ReferencedAssemblies.Add("System.dll");
			compilerParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			CompilerResults results = compiler.CompileAssemblyFromSource(
                             compilerParams,functionCode);
			if (results.Errors.Count > 0){
				MessageBox.Show(results.Errors[0].ErrorText);
				//foreach (CompilerError error in results.Errors)
				//	DotNetScriptEngine.LogAllErrMsgs("Compine Error:"+error.ErrorText); 
				return;
			}
			Assembly assembly = results.CompiledAssembly;	
			//Use reflection to call the Main function in the assembly
			ScriptEngine.RunScript(assembly, "Main");		
			*/

		}

		

		
	}
}
