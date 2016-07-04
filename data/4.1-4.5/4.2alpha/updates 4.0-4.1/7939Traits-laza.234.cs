"Change Set:		7939Traits-laza.234Traits-laza.234:This fixes Mantis bug 0007090: 'TraitsTests pollutes ProtoObject subclasses' by filing in TraitsTestSubclassesCleanup-M7090.1.cs"!!TraitsResource methodsFor: 'as yet unclassified' stamp: 'noha 6/11/2008 18:42'!setUpTwoLevelRequiresFixture	self c4: (self 				createClassNamed: #C4				superclass: ProtoObject				uses: { }).	ProtoObject removeSubclass: self c4.	self c4 superclass: nil.	self c5: (self 				createClassNamed: #C5				superclass: self c4				uses: { }).	self c4 compile: 'foo ^self blew' classified: #accessing.	self c5 compile: 'foo ^self blah' classified: #accessing! !!RequiresTestCase methodsFor: 'as yet unclassified' stamp: 'noha 6/11/2008 18:45'!testSins	| caa cab cac cad |	caa := self 				createClassNamed: #CAA				superclass: ProtoObject				uses: { }.	ProtoObject removeSubclass: caa.	caa superclass: nil.	cab := self 				createClassNamed: #CAB				superclass: caa				uses: {}.	cac := self 				createClassNamed: #CAC				superclass: cab				uses: {}.	cad := self 				createClassNamed: #CAD				superclass: cac				uses: { }.	caa compile: 'ma self foo'.	caa compile: 'md self explicitRequirement'.	cac compile: 'mb self bar'.	self noteInterestsFor: cad.	self assert: (cad requiredSelectors = (Set withAll: #(foo bar md))).	cab compile: 'mc ^3'.	self assert: (cad requiredSelectors = (Set withAll: #(foo bar md))).	self loseInterestsFor: cad.! !!TraitsResource methodsFor: 'as yet unclassified' stamp: 'noha 6/11/2008 18:41'!setUpTranslatingRequiresFixture	self c6: (self 				createClassNamed: #C6				superclass: ProtoObject				uses: { }).	ProtoObject removeSubclass: self c6.	self c6 superclass: nil.	self c7: (self 				createClassNamed: #C7				superclass: self c6				uses: { }).	self c8: (self 				createClassNamed: #C8				superclass: self c7				uses: { }).	self c6 compile: 'foo ^self x' classified: #accessing.	self c7 compile: 'foo ^3' classified: #accessing.	self c7 compile: 'bar ^super foo' classified: #accessing.	self c8 compile: 'bar ^self blah' classified: #accessing! !!TraitsResource methodsFor: 'as yet unclassified' stamp: 'noha 6/11/2008 18:42'!setUpTrivialRequiresFixture	self c3: (self 				createClassNamed: #C3				superclass: ProtoObject				uses: { }).	ProtoObject removeSubclass: self c3.	self c3 superclass: nil.	self c3 compile: 'foo ^self bla' classified: #accessing! !