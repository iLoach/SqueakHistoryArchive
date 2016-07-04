"Change Set:		9410ReleaseBuilder-edc.48ReleaseBuilder-edc.48:ReleaseBuilderFor3dot11 new createCompressedSources is fixed and make the dir and files ReleaseBuilder-dtl.47:Remove explicit reference to MVC ListPargraph from ReleaseBuilderTrunk class>>prepareNewBuild."!!ReleaseBuilderTrunk class methodsFor: 'scripts' stamp: 'dtl 2/14/2010 19:53'!prepareNewBuild "ReleaseBuilderTrunk prepareNewBuild"	"Prepare everything that should be done for a new image build"	Smalltalk cleanUpUndoCommands.	Undeclared removeUnreferencedKeys.	StandardScriptingSystem initialize.	GradientFillStyle initPixelRampCache.	(NaturalLanguageFormTranslator bindingOf: #CachedTranslations) value: nil.	(NaturalLanguageTranslator bindingOf: #CachedTranslations) value: nil.	(NaturalLanguageTranslator bindingOf: #AllKnownPhrases) value: nil.	PaintBoxMorph classPool	at: #ColorChart put: nil.	(Utilities bindingOf: #ScrapsBook) value: nil.	CommandHistory resetAllHistory.	Smalltalk at: #Browser ifPresent:[:br| br initialize].	DataStream initialize.	Smalltalk at: #ListParagraph ifPresent: [:lp | lp initialize].	PopUpMenu initialize.	Smalltalk forgetDoIts.	Smalltalk flushClassNameCache.	ScrollBar initializeImagesCache. 	Behavior flushObsoleteSubclasses.	AppRegistry withAllSubclassesDo:[:reg| reg removeObsolete].	FileServices removeObsolete. 	ExternalDropHandler resetRegisteredHandlers.	SystemOrganization removeEmptyCategories.	MCFileBasedRepository flushAllCaches.	MCMethodDefinition shutDown.	MCDefinition clearInstances.	HashedCollection rehashAll.	ChangeSet removeChangeSetsNamedSuchThat:		[:cs| cs name ~= ChangeSet current name].	ChangeSet current clear.	ChangeSet current name: 'Unnamed1'.	Smalltalk garbageCollect.	3 timesRepeat: [ 		Smalltalk garbageCollect. 		Symbol compactSymbolTable.	].! !!ReleaseBuilderFor3dot11 methodsFor: 'sources managment' stamp: 'edc 2/16/2010 11:25'!createCompressedSources" ReleaseBuilderFor3dot11 new createCompressedSources"ProtoObject allSubclassesWithLevelDo:[:cl :l| | dir | 	dir := self createDirIfnotExists:cl category.			Cursor write showWhile: [ | zipped nameToUse unzipped buffer |		nameToUse :=  cl printString , FileDirectory dot, ImageSegment compressedFileExtension. 		(dir fileExists: nameToUse) ifFalse:[			unzipped :=RWBinaryOrTextStream on: ''.			unzipped header; timeStamp.	 cl  fileOutOn: unzipped moveSource: false toFile: 0.	unzipped trailer.				unzipped reset.			zipped := dir newFileNamed: (nameToUse).	zipped binary.	zipped := GZipWriteStream on: zipped.	buffer := ByteArray new: 50000.	'Compressing ', nameToUse displayProgressAt: Sensor cursorPoint		from: 0 to: unzipped size		during:[:bar|			[unzipped atEnd] whileFalse:[				bar value: unzipped position.				zipped nextPutAll: (unzipped nextInto: buffer)].			zipped close.			unzipped close]]]] startingLevel: 0! !