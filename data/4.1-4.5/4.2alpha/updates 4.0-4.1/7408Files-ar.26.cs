"Change Set:		7408Files-ar.26Files-ar.26:UIManagerization. Replaces all the trivial references to PopUpMenu, SelectionMenu, CustomMenu, and FillInTheBlank.Files-mha.24:make tests more disciplined: remove files created during testingFiles-mha.25:make tests more disciplined: remove files created during testing"!!FileDirectory methodsFor: 'file operations' stamp: 'ar 8/6/2009 18:20'!rename: oldFileName toBe: newFileName	| selection oldName newName |	"Rename the file of the given name to the new name. Fail if there is no file of the old name or if there is an existing file with the new name."	"Modified for retry after GC ar 3/21/98 18:09"	oldName := self fullNameFor: oldFileName.	newName := self fullNameFor: newFileName.	(StandardFileStream 		retryWithGC:[self primRename: oldName asVmPathName to: newName asVmPathName]		until:[:result| result notNil]		forFileNamed: oldName) ~~ nil ifTrue:[^self].	(self fileExists: oldFileName) ifFalse:[		^self error:'Attempt to rename a non-existent file'.	].	(self fileExists: newFileName) ifTrue:[		selection := UIManager default chooseFrom:#('delete old version' 'cancel')				title: 'Trying to rename a file to be', newFileName , 'and it already exists.'.		selection = 1 ifTrue:			[self deleteFileNamed: newFileName.			^ self rename: oldFileName toBe: newFileName]].	^self error:'Failed to rename file'.! !!FileDirectoryTest methodsFor: 'existence tests' stamp: 'mha 7/30/2009 11:24'!testAttemptExistenceCheckWhenFile	"How should a FileDirectory instance respond with an existent file name?"		| directory filename |		filename := 'aTestFile'.	FileDirectory default forceNewFileNamed: filename.	directory := FileDirectory default directoryNamed: filename.	self shouldnt: [directory exists] description: 'Files are not directories.'.		"clean up disk"	FileDirectory default deleteFileNamed: filename ifAbsent: [ ]! !!FileStreamTest methodsFor: 'as yet unclassified' stamp: 'mha 7/30/2009 11:25'!testDetectFileDo	"Mantis #1838"		| filename |	filename := 'filestream.tst'.		[(FileDirectory default forceNewFileNamed: filename)		nextPutAll: '42';		close.			FileStream 		detectFile: [FileDirectory default oldFileNamed: filename]		do: [:file |			self assert: file notNil.			self deny: file closed.			self assert: file contentsOfEntireFile = '42']]			ensure: [FileDirectory default deleteFileNamed: filename ifAbsent: [] ]! !!StandardFileStream class methodsFor: 'error handling' stamp: 'ar 8/6/2009 20:45'!readOnlyFileDoesNotExistUserHandling: fullFileName	| dir files choices selection newName fileName |	dir := FileDirectory forFileName: fullFileName.	files := dir fileNames.	fileName := FileDirectory localNameFor: fullFileName.	choices := fileName correctAgainst: files.	choices add: 'Choose another name'.	choices add: 'Cancel'.	selection := UIManager default chooseFrom: choices lines: (Array with: 5)		title: (FileDirectory localNameFor: fullFileName), 'does not exist.'.	selection = choices size ifTrue:["cancel" ^ nil "should we raise another exception here?"].	selection < (choices size - 1) ifTrue: [		newName := (dir pathName , FileDirectory slash , (choices at: selection))].	selection = (choices size - 1) ifTrue: [		newName := UIManager default 							request: 'Enter a new file name' 							initialAnswer: fileName].	newName = '' ifFalse: [^ self readOnlyFileNamed: (self fullName: newName)].	^ self error: 'Could not open a file'! !!StandardFileStream class methodsFor: 'error handling' stamp: 'ar 8/6/2009 20:45'!fileDoesNotExistUserHandling: fullFileName	| selection newName |	selection := UIManager default chooseFrom: {		'create a new file' translated.		'choose another name' translated.		'cancel' translated	} title: (FileDirectory localNameFor: fullFileName) , 'does not exist.'.	selection = 1 ifTrue:		[^ self new open: fullFileName forWrite: true].	selection = 2 ifTrue:		[ newName := UIManager default request: 'Enter a new file name'						initialAnswer:  fullFileName.		^ self oldFileNamed:			(self fullName: newName)].	self halt! !!StandardFileStream class methodsFor: 'error handling' stamp: 'ar 8/6/2009 20:45'!fileExistsUserHandling: fullFileName	| dir localName choice newName newFullFileName |	dir := FileDirectory forFileName: fullFileName.	localName := FileDirectory localNameFor: fullFileName.	choice := UIManager default chooseFrom:{		'overwrite that file'.		'choose another name'.		'cancel'	} title: localName, 'already exists.'.	choice = 1 ifTrue: [		dir deleteFileNamed: localName			ifAbsent: [self error: 'Could not delete the old version of that file'].		^ self new open: fullFileName forWrite: true].	choice = 2 ifTrue: [		newName := UIManager default request: 'Enter a new file name' initialAnswer: fullFileName.		newFullFileName := self fullName: newName.		^ self newFileNamed: newFullFileName].	self error: 'Please close this to abort file opening'! !!FileDirectory class methodsFor: 'name utilities' stamp: 'ar 8/6/2009 20:40'!searchAllFilesForAString	"Prompt the user for a search string, and a starting directory. Search the contents of all files in the starting directory and its subdirectories for the search string (case-insensitive search.)	List the paths of files in which it is found on the Transcript.	By Stewart MacLean 5/00; subsequently moved to FileDirectory class-side, and refactored to call FileDirectory.filesContaining:caseSensitive:"	| searchString dir |	searchString := UIManager default request: 'Enter search string'.	searchString isEmpty ifTrue: [^nil].	Transcript cr; show: 'Searching for ', searchString printString, ' ...'.	(dir := PluggableFileList getFolderDialog open) ifNotNil:		[(dir filesContaining: searchString caseSensitive: false) do:				[:pathname | Transcript cr; show: pathname]].	Transcript cr; show: 'Finished searching for ', searchString printString	"FileDirectory searchAllFilesForAString"! !