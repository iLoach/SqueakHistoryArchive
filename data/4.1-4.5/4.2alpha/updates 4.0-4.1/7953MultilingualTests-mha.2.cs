"Change Set:		7953MultilingualTests-mha.2MultilingualTests-mha.2:made MultiByteFileStream test clean up properly after itself"!!MultiByteFileStreamTest methodsFor: 'testing' stamp: 'mha 10/12/2009 09:39'!testBinaryUpTo	"This is a non regression test for bug http://bugs.squeak.org/view.php?id=6933"		| foo fileName |		fileName := 'foobug6933'.		foo := MultiByteFileStream forceNewFileNamed: fileName.	[foo binary.	foo nextPutAll: #(1 2 3 4) asByteArray] ensure: [foo close].	foo := MultiByteFileStream oldFileNamed: fileName.	[foo binary.	self assert: (foo upTo: 3) = #(1 2 ) asByteArray] ensure: [foo close].	FileDirectory default deleteFileNamed: fileName! !