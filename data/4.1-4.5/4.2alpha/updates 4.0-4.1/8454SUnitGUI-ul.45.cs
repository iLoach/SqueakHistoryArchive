"Change Set:		8454SUnitGUI-ul.45SUnitGUI-ul.45:- replace sends of #ifNotNilDo: to #ifNotNil:, #ifNil:ifNotNilDo: to #ifNil:ifNotNil:, #ifNotNilDo:ifNil: to #ifNotNil:ifNil:"!!TestRunner methodsFor: 'utilities' stamp: 'ul 12/12/2009 14:05'!findCategories	| visible |	visible := Set new.	self baseClass withAllSubclassesDo: [ :each |		each category ifNotNil: [ :category |			visible add: category ] ].	^ Array streamContents: [ :stream |		Smalltalk organization categories do: [ :each |			(visible includes: each)				ifTrue: [ stream nextPut: each ] ] ].! !