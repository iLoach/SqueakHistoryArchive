"Change Set:		8934ToolBuilder-Kernel-mtf.30ToolBuilder-Kernel-mtf.30:Merged in Cobalt branch of ToolBuilder. This includes the addition of a few properties, and moving the help ivar all the way to the top of the spec heirarchy, since so many subclasses used it"!PluggableWidgetSpec subclass: #PluggableListSpec	instanceVariableNames: 'list getIndex setIndex getSelected setSelected menu keyPress autoDeselect dragItem dropItem dropAccept doubleClick listSize listItem'	classVariableNames: ''	poolDictionaries: ''	category: 'ToolBuilder-Kernel'!ToolBuilderSpec subclass: #PluggableMenuItemSpec	instanceVariableNames: 'label action checked enabled separator subMenu'	classVariableNames: ''	poolDictionaries: ''	category: 'ToolBuilder-Kernel'!PluggableWidgetSpec subclass: #PluggableTreeSpec	instanceVariableNames: 'roots getSelectedPath setSelected getChildren hasChildren label icon unusedVar menu keyPress wantsDrop dropItem dropAccept autoDeselect dragItem'	classVariableNames: ''	poolDictionaries: ''	category: 'ToolBuilder-Kernel'!PluggableCompositeSpec subclass: #PluggableWindowSpec	instanceVariableNames: 'label extent closeAction isDialog'	classVariableNames: ''	poolDictionaries: ''	category: 'ToolBuilder-Kernel'!PluggableWidgetSpec subclass: #PluggableButtonSpec	instanceVariableNames: 'action label state enabled color'	classVariableNames: ''	poolDictionaries: ''	category: 'ToolBuilder-Kernel'!Object subclass: #ToolBuilderSpec	instanceVariableNames: 'name help'	classVariableNames: ''	poolDictionaries: ''	category: 'ToolBuilder-Kernel'!!PluggableTreeSpec methodsFor: 'accessing' stamp: 'mvdg 2/11/2007 13:47'!dragItem: aSymbol	"Set the selector for dragging an item"	dragItem := aSymbol! !!PluggableTreeSpec methodsFor: 'accessing' stamp: 'mvdg 2/11/2007 13:47'!dragItem	^ dragItem.! !!PluggableListSpec methodsFor: 'accessing' stamp: 'mtf 9/27/2007 11:12'!listSize: aSymbol	"Indicate the selector for retrieving the list size"	listSize := aSymbol.! !!UIManager methodsFor: 'ui requests' stamp: 'dc 1/10/2008 08:09'!confirm: queryString trueChoice: trueChoice falseChoice: falseChoice 	"Put up a yes/no menu with caption queryString. The actual wording for the two choices will be as provided in the trueChoice and falseChoice parameters. Answer true if the response is the true-choice, false if it's the false-choice.	This is a modal question -- the user must respond one way or the other."	^self subclassResponsibility! !!PluggableListSpec methodsFor: 'accessing' stamp: 'mtf 9/27/2007 11:13'!listItem: aSymbol	"Indicate the selector for retrieving the list element"	listItem := aSymbol.! !!PluggableListSpec methodsFor: 'accessing' stamp: 'mtf 9/27/2007 11:13'!listItem	"Answer the selector for retrieving the list element"	^listItem! !!PluggableListSpec methodsFor: 'accessing' stamp: 'mtf 9/27/2007 11:11'!listSize	"Answer the selector for retrieving the list size"	^listSize! !!ToolBuilderSpec methodsFor: 'accessing' stamp: 'btr 11/26/2006 12:38'!help: aSymbol 	"Indicate the message to retrieve the help texts of this element."	help := aSymbol! !!UIManager methodsFor: 'ui requests' stamp: 'jrd 5/12/2009 18:02'!confirm: queryString label: titleString		^self subclassResponsibility! !!PluggableWindowSpec methodsFor: 'accessing' stamp: 'jrd 11/28/2008 01:27'!isDialog: val	isDialog := val! !!PluggableTreeSpec methodsFor: 'accessing' stamp: 'mvdg 3/21/2008 18:09'!autoDeselect: aBool	"Indicate whether this tree can be automatically deselected"	autoDeselect := aBool.! !!PluggableWindowSpec methodsFor: 'accessing' stamp: 'jrd 11/28/2008 01:32'!isDialog	^isDialog ifNil: [^false].! !!ToolBuilderSpec methodsFor: 'accessing' stamp: 'btr 11/26/2006 12:37'!help	"Answer the message to get the help texts of this element."	^ help! !PluggableTreeSpec removeSelector: #help!PluggableButtonSpec removeSelector: #help:!PluggableMenuItemSpec removeSelector: #help:!PluggableButtonSpec removeSelector: #help!PluggableMenuItemSpec removeSelector: #help!PluggableTreeSpec removeSelector: #help:!