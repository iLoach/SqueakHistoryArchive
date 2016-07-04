"Change Set:		8723Tools-ar.156Tools-ar.156:Make services unloadable: Provide a menu hook which ServiceGUI can utilize to modify various menus in browsers instead of hacking those methods directly.Tools-ar.155:Don't use definitionST80."!!Browser methodsFor: 'message category functions' stamp: 'ar 1/2/2010 15:16'!messageCategoryMenu: aMenu	(self menuHook: aMenu named: #messageCategoryMenu shifted: false) ifTrue:[^aMenu].	^ aMenu labels:'browseprintOutfileOutreorganizealphabetizeremove empty categoriescategorize all uncategorizednew category...rename...remove'		lines: #(3 8)		selections:		#(buildMessageCategoryBrowser printOutMessageCategories fileOutMessageCategories		editMessageCategories alphabetizeMessageCategories removeEmptyCategories		categorizeAllUncategorizedMethods addCategory renameCategory removeMessageCategory)! !!CodeHolder methodsFor: 'misc' stamp: 'ar 1/2/2010 15:25'!menuHook: aMenu named: aSymbol shifted: aBool	"Provide a hook for supplemental menu items. The return value indicates	whether to only use the supplemental menu or wether to add the regular	menu items to the menu. The only known user is ServiceGUI."	^false! !!Browser methodsFor: 'message functions' stamp: 'ar 1/2/2010 15:18'!messageListMenu: aMenu shifted: shifted 	"Answer the message-list menu"	(self menuHook: aMenu named: #messageListMenu shifted: shifted) ifTrue:[^aMenu].	shifted ifTrue: [^ self shiftedMessageListMenu: aMenu].	aMenu addList: #(			('what to show...'			offerWhatToShowMenu)			('toggle break on entry'		toggleBreakOnEntry)			-			('browse full (b)' 			browseMethodFull)			('browse hierarchy (h)'			classHierarchy)			('browse method (O)'			openSingleMessageBrowser)			('browse protocol (p)'			browseFullProtocol)			-			('fileOut'				fileOutMessage)			('printOut'				printOutMessage)			-			('senders of... (n)'			browseSendersOfMessages)			('implementors of... (m)'		browseMessages)			('inheritance (i)'			methodHierarchy)			('tile scriptor'			openSyntaxView)			('versions (v)'				browseVersions)			-			('inst var refs...'			browseInstVarRefs)			('inst var defs...'			browseInstVarDefs)			('class var refs...'			browseClassVarRefs)			('class variables'			browseClassVariables)			('class refs (N)'			browseClassRefs)			-			('remove method (x)'			removeMessage)			-			('more...'				shiftedYellowButtonActivity)).	^ aMenu! !!Browser methodsFor: 'system category functions' stamp: 'ar 1/2/2010 15:18'!systemCategoryMenu: aMenu	(self menuHook: aMenu named: #systemCategoryMenu shifted: false) ifTrue:[^aMenu].	^ aMenu labels:'find class... (f)recent classes... (r)browse allbrowseprintOutfileOutreorganizealphabetizeupdateadd item...rename...remove' 	lines: #(2 4 6 8)	selections:		#(findClass recent browseAllClasses buildSystemCategoryBrowser		printOutSystemCategory fileOutSystemCategory		editSystemCategories alphabetizeSystemCategories updateSystemCategories		addSystemCategory renameSystemCategory removeSystemCategory )! !!Browser methodsFor: 'code pane' stamp: 'ar 1/2/2010 15:15'!codePaneMenu: aMenu shifted: shifted 	(self menuHook: aMenu named: #codePaneMenu shifted: shifted) ifTrue:[^aMenu].	^super codePaneMenu: aMenu shifted: shifted.! !!MessageSet methodsFor: 'contents' stamp: 'ar 1/2/2010 03:41'!selectedMessage	"Answer the source method for the currently selected message."		self setClassAndSelectorIn: [:class :selector | | source | 		class ifNil: [^ 'Class vanished'].		selector first isUppercase ifTrue:			[selector == #Comment ifTrue:				[currentCompiledMethod := class organization commentRemoteStr.				^ class comment].			selector == #Definition ifTrue:				[^ class definition].			selector == #Hierarchy ifTrue: [^ class printHierarchy]].		source := class sourceMethodAt: selector ifAbsent:			[currentCompiledMethod := nil.			^ 'Missing'].		self showingDecompile ifTrue: [^ self decompiledSourceIntoContents].		currentCompiledMethod := class compiledMethodAt: selector ifAbsent: [nil].		self showingDocumentation ifTrue: [^ self commentContents].	source := self sourceStringPrettifiedAndDiffed.	^ source asText makeSelectorBoldIn: class]! !!Browser methodsFor: 'class functions' stamp: 'ar 1/2/2010 15:14'!classListMenu: aMenu shifted: shifted	"Set up the menu to apply to the receiver's class list, honoring the #shifted boolean"	(self menuHook: aMenu named: #classListMenu shifted: shifted) ifTrue:[^aMenu].	shifted ifTrue:[^ self shiftedClassListMenu: aMenu].	aMenu addList: #(		-		('browse full (b)'			browseMethodFull)		('browse hierarchy (h)'		spawnHierarchy)		('browse protocol (p)'		browseFullProtocol)		-		('printOut'					printOutClass)		('fileOut'					fileOutClass)		-		('show hierarchy'			hierarchy)		('show definition'			editClass)		('show comment'			editComment)		-		('inst var refs...'			browseInstVarRefs)		('inst var defs...'			browseInstVarDefs)		-		('class var refs...'			browseClassVarRefs)		('class vars'					browseClassVariables)		('class refs (N)'				browseClassRefs)		-		('rename class ...'			renameClass)		('copy class'				copyClass)		('remove class (x)'			removeClass)		-		('find method...'				findMethod)		('find method wildcard...'	findMethodWithWildcard)		-		('more...'					offerShiftedClassListMenu)).	^ aMenu! !