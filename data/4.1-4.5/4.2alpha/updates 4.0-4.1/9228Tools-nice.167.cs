"Change Set:		9228Tools-nice.167Tools-nice.167:1) move some temp assignment outside blocks2) remove some now useless fixTemps3) add a pair of translated"!!ChangeList class methodsFor: 'public access' stamp: 'nice 1/19/2010 01:52'!browseRecent: charCount on: origChangesFile 	"Opens a changeList on the end of the specified changes log file"	| changeList end changesFile |	changesFile := origChangesFile readOnlyCopy.	changesFile setConverterForCode.	end := changesFile size.	changeList := Cursor read		showWhile: [self new						scanFile: changesFile						from: (0 max: end - charCount)						to: end].	changesFile close.	self		open: changeList		name: 'Recent changes'		multiSelect: true! !!ChangeList methodsFor: 'menu actions' stamp: 'nice 1/19/2010 01:51'!selectNewMethods	"Selects all method definitions for which there is no counterpart method in the current image"	Cursor read showWhile: 		[ | change class |		1 to: changeList size do:			[:i | change := changeList at: i.			listSelections at: i put:				((change type = #method and:					[((class := change methodClass) isNil) or:						[(class includesSelector: change methodSelector) not]]))]].	self changed: #allSelections! !!ChangeList class methodsFor: 'public access' stamp: 'nice 1/17/2010 22:19'!browseRecentLogOn: origChangesFile startingFrom: initialPos 	"Prompt with a menu of how far back to go when browsing a changes file."	| end banners positions pos chunk i changesFile |	changesFile := origChangesFile readOnlyCopy.	banners := OrderedCollection new.	positions := OrderedCollection new.	end := changesFile size.	changesFile setConverterForCode.	pos := initialPos.	[pos = 0		or: [banners size > 20]]		whileFalse: [changesFile position: pos.			chunk := changesFile nextChunk.			i := chunk indexOfSubCollection: 'priorSource: ' startingAt: 1.			i > 0				ifTrue: [positions addLast: pos.					banners						addLast: (chunk copyFrom: 5 to: i - 2).					pos := Number								readFrom: (chunk copyFrom: i + 13 to: chunk size)]				ifFalse: [pos := 0]].	changesFile close.	banners size == 0 ifTrue: [^ self inform: 'this image has never been savedsince changes were compressed' translated].	pos := UIManager default chooseFrom:  banners values: positions title: 'Browse as far back as...' translated.	pos ifNil: [^ self].	self browseRecent: end - pos on: origChangesFile! !!ChangeList class methodsFor: 'public access' stamp: 'nice 1/19/2010 01:52'!browseStream: changesFile	"Opens a changeList on a fileStream"	| changeList charCount |	changesFile readOnly.	changesFile setConverterForCode.	charCount := changesFile size.	charCount > 1000000 ifTrue:		[(self confirm: 'The file ', changesFile name , 'is really long (' , charCount printString , ' characters).Would you prefer to view only the last million characters?')			ifTrue: [charCount := 1000000]].	"changesFile setEncoderForSourceCodeNamed: changesFile name."	changeList := Cursor read showWhile:		[self new			scanFile: changesFile from: changesFile size-charCount to: changesFile size].	changesFile close.	self open: changeList name: changesFile localName , ' log' multiSelect: true! !!ChangeList methodsFor: 'menu actions' stamp: 'nice 1/19/2010 01:52'!selectUnchangedMethods	"Selects all method definitions for which there is already a method in the current image, whose source is exactly the same.  9/18/96 sw"	Cursor read showWhile: 	[ | class change |	1 to: changeList size do:		[:i | change := changeList at: i.		listSelections at: i put:			((change type = #method and:				[(class := change methodClass) notNil]) and:					[(class includesSelector: change methodSelector) and:						[change string withBlanksCondensed = (class sourceCodeAt: change methodSelector) asString withBlanksCondensed ]])]].	self changed: #allSelections! !!ChangeList methodsFor: 'menu actions' stamp: 'nice 1/19/2010 01:51'!selectConflicts: changeSetOrList	"Selects all method definitions for which there is ALSO an entry in the specified changeSet or changList"	Cursor read showWhile: 	[ | change class |	(changeSetOrList isKindOf: ChangeSet) ifTrue: [	1 to: changeList size do:		[:i | change := changeList at: i.		listSelections at: i put:			(change type = #method			and: [(class := change methodClass) notNil			and: [(changeSetOrList atSelector: change methodSelector						class: class) ~~ #none]])]]	ifFalse: ["a ChangeList"	1 to: changeList size do:		[:i | change := changeList at: i.		listSelections at: i put:			(change type = #method			and: [(class := change methodClass) notNil			and: [changeSetOrList list includes: (list at: i)]])]]	].	self changed: #allSelections! !!ChangeList methodsFor: 'menu actions' stamp: 'nice 1/19/2010 01:51'!selectConflicts	"Selects all method definitions for which there is ALSO an entry in changes"	Cursor read showWhile: 	[ | change class |	1 to: changeList size do:		[:i | change := changeList at: i.		listSelections at: i put:			(change type = #method			and: [(class := change methodClass) notNil			and: [(ChangeSet current atSelector: change methodSelector						class: class) ~~ #none]])]].	self changed: #allSelections! !!FileChooser methodsFor: 'initialization' stamp: 'nice 1/11/2010 20:55'!setSuffixes: aList	self fileSelectionBlock:  [:entry :myPattern |			entry isDirectory				ifTrue:					[false]				ifFalse:					[aList includes: (FileDirectory extensionFor: entry name asLowercase)]]! !!ChangeList methodsFor: 'menu actions' stamp: 'nice 1/19/2010 01:51'!selectAllConflicts	"Selects all method definitions in the receiver which are also in any existing change set in the system.  This makes no statement about whether the content of the methods differ, only whether there is a change represented."	Cursor read showWhile: 		[ | aClass aChange |		1 to: changeList size do:			[:i | aChange := changeList at: i.			listSelections at: i put:				(aChange type = #method				and: [(aClass := aChange methodClass) notNil				and: [ChangesOrganizer doesAnyChangeSetHaveClass: aClass andSelector:  aChange methodSelector]])]].	self changed: #allSelections! !!ChangeList methodsFor: 'menu actions' stamp: 'nice 1/19/2010 01:50'!browseCurrentVersionsOfSelections	"Opens a message-list browser on the current in-memory versions of all methods that are currently seleted"	| aList |	aList := OrderedCollection new.	Cursor read showWhile: [		1 to: changeList size do: [:i |			(listSelections at: i) ifTrue: [				| aClass aChange |				aChange := changeList at: i.				(aChange type = #method					and: [(aClass := aChange methodClass) notNil					and: [aClass includesSelector: aChange methodSelector]])						ifTrue: [							aList add: (								MethodReference new									setStandardClass: aClass  									methodSymbol: aChange methodSelector							)						]]]].	aList size == 0 ifTrue: [^ self inform: 'no selected methods have in-memory counterparts'].	MessageSet 		openMessageList: aList 		name: 'Current versions of selected methods in ', file localName! !