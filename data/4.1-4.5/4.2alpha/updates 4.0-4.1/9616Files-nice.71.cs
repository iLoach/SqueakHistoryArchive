"Change Set:		9616Files-nice.71Files-nice.71:1) Move File exceptions to Files package2) Integrate FileWriteError from Cuis"!FileStreamException subclass: #FileExistsException	instanceVariableNames: 'fileClass'	classVariableNames: ''	poolDictionaries: ''	category: 'Files-Exceptions'!FileStreamException subclass: #FileWriteError	instanceVariableNames: ''	classVariableNames: ''	poolDictionaries: ''	category: 'Files-Exceptions'!FileStreamException subclass: #CannotDeleteFileException	instanceVariableNames: ''	classVariableNames: ''	poolDictionaries: ''	category: 'Files-Exceptions'!Error subclass: #FileStreamException	instanceVariableNames: 'fileName'	classVariableNames: ''	poolDictionaries: ''	category: 'Files-Exceptions'!FileStreamException subclass: #FileDoesNotExistException	instanceVariableNames: 'readOnly'	classVariableNames: ''	poolDictionaries: ''	category: 'Files-Exceptions'!!StandardFileStream methodsFor: 'primitives' stamp: 'jmv 12/14/2009 13:57'!primWrite: id from: stringOrByteArray startingAt: startIndex count: count	"Write count bytes onto this file from the given string or byte array starting at the given index. Answer the number of bytes written."	<primitive: 'primitiveFileWrite' module: 'FilePlugin'>	(FileWriteError fileName: name)		signal: (self closed			ifTrue: [ 'File ', name, ' is closed' ]			ifFalse: [ 'File ', name, 'write failed' ])! !!FileStreamException methodsFor: 'exceptionDescription' stamp: 'mir 2/23/2000 20:13'!isResumable	"Determine whether an exception is resumable."	^true! !!FileStreamException methodsFor: 'exceptionDescription' stamp: 'mir 2/25/2000 17:29'!fileName	^fileName! !!FileDoesNotExistException methodsFor: 'accessing' stamp: 'mir 7/25/2000 16:40'!readOnly: aBoolean	readOnly := aBoolean! !!FileDoesNotExistException class methodsFor: 'examples' stamp: 'mir 2/29/2000 11:44'!example	"FileDoesNotExistException example"	| result |	result := [(StandardFileStream readOnlyFileNamed: 'error42.log') contentsOfEntireFile]		on: FileDoesNotExistException		do: [:ex | 'No error log'].	Transcript show: result; cr! !!FileDoesNotExistException methodsFor: 'accessing' stamp: 'mir 7/25/2000 16:41'!readOnly	^readOnly == true! !!FileStreamException methodsFor: 'exceptionDescription' stamp: 'mir 2/23/2000 20:14'!messageText		"Return an exception's message text."	^messageText == nil		ifTrue: [fileName printString]		ifFalse: [messageText]! !!FileExistsException methodsFor: 'accessing' stamp: 'LC 10/24/2001 21:42'!fileClass: aClass	fileClass := aClass! !!FileExistsException methodsFor: 'accessing' stamp: 'LC 10/24/2001 21:49'!fileClass	^ fileClass ifNil: [StandardFileStream]! !!FileStreamException methodsFor: 'exceptionBuilder' stamp: 'mir 2/23/2000 20:13'!fileName: aFileName	fileName := aFileName! !!FileDoesNotExistException methodsFor: 'exceptionDescription' stamp: 'mir 7/25/2000 18:22'!defaultAction	"The default action taken if the exception is signaled."	^self readOnly		ifTrue: [StandardFileStream readOnlyFileDoesNotExistUserHandling: self fileName]		ifFalse: [StandardFileStream fileDoesNotExistUserHandling: self fileName]! !!FileStreamException class methodsFor: 'exceptionInstantiator' stamp: 'mir 2/23/2000 20:12'!fileName: aFileName	^self new fileName: aFileName! !!FileExistsException methodsFor: 'exceptionDescription' stamp: 'LC 10/24/2001 21:50'!defaultAction	"The default action taken if the exception is signaled."	^ self fileClass fileExistsUserHandling: self fileName! !!FileExistsException class methodsFor: 'exceptionInstantiator' stamp: 'LC 10/24/2001 21:50'!fileName: aFileName fileClass: aClass 	^ self new		fileName: aFileName;		fileClass: aClass! !