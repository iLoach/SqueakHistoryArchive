"Change Set:		8937ToolsTests-ul.3ToolsTests-ul.3:- code critics"!!FileListTest methodsFor: 'private' stamp: 'ul 1/11/2010 07:08'!checkIsServiceIsFromDummyTool: service		^ (service instVarNamed: #provider) = DummyToolWorkingWithFileList and: [		service label = 'menu label' and: [		(service instVarNamed: #selector) = #loadAFileForTheDummyTool: ] ]! !