bulk insert CsvTable_dumb 
	from 'D:\Testt\CsvTest.csv'
	with (
		Firstrow = 2,
		fieldterminator = ',',
		rowterminator = '\n'
		)

Insert into dbo.CsvTable(LogTime,
	Action,
	FolderPath, 
	Filename,
	Username,
	IpAdress,
	XferSize,
	Duration,
	AgentBrand,
	AgentVersion,
	Error)
select 
	LogTime = TRY_PARSE(REPLACE(LogTime,'"', '') AS DATETIME USING 'en-gb'),
	Action =(REPLACE(Action,'"', '')),
	FolderPath = (REPLACE(FolderPath,'"', '')),
	Filename = (REPLACE(Filename,'"', '')),
	Username = (REPLACE(Username,'"', '')),
	IpAdress = (REPLACE(IpAdress,'"', '')),
	XferSize = CONVERT(int, (REPLACE(XferSize,'"', ''))),
	Duration = CONVERT(float, (REPLACE(Duration,'"', ''))),
	AgentBrand = REPLACE(AgentBrand,'"', ''),
	AgentVersion = REPLACE(AgentVersion,'"', ''),
	Error = CONVERT(int, (Replace((REPLACE(Error,'"', '')),',','')))
 from CsvTable_dumb

 Delete from CsvTable_dumb
