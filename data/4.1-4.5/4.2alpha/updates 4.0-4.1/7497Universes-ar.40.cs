"Change Set:		7497Universes-ar.40Universes-ar.40:UIManagerization. Replaces all the trivial references to PopUpMenu, SelectionMenu, CustomMenu, and FillInTheBlank."!!UUniverseEditor class methodsFor: 'instance creation' stamp: 'ar 8/6/2009 19:17'!new	| choice choices |	choices := UUniverse systemUniverse standardUniverses asArray.	choices isEmpty ifTrue: [ ^self error: 'no standard universes installed' ].	choices size = 1 ifTrue: [		choice := choices anyOne ]	ifFalse: [		choice := UIManager default 				chooseFrom: (choices collect: [ :u | u description ])				values: choices				title: 'edit which universe?'.		choice ifNil: [ ^self ] ].		^self forUniverse: choice! !