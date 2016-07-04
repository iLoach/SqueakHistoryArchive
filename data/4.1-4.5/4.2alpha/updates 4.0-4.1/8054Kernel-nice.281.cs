"Change Set:		8054Kernel-nice.281Kernel-nice.281:Pick in Pharo one forgotten changes of Eliot closure...That makes more ClosureCompilerTest greenKernel-jcg.279:#notYetImplemented now raises a NotYetImplemented error, instead of popping up a notifier. (The NYI class was introduced in Exceptions-jcg.14).Kernel-nice.280:Refactor #whichClassDefinesClassVar:The class var name is a Symbol, so- first test if such a Symbol exists- second test directly = aSymbol rather than converting asString"!!Object methodsFor: 'user interface' stamp: 'jcg 10/21/2009 00:53'!notYetImplemented	NotYetImplemented signal	! !!Behavior methodsFor: 'queries' stamp: 'nice 10/22/2009 14:28'!whichClassDefinesClassVar: aString 	Symbol hasInterned: aString ifTrue: [ :aSymbol |		^self whichSuperclassSatisfies: 			[:aClass | 			aClass classVarNames anySatisfy: [:each | each = aSymbol]]].	^nil! !!ContextPart methodsFor: 'system simulation' stamp: 'eem 6/16/2008 15:39'!runSimulated: aBlock contextAtEachStep: block2	"Simulate the execution of the argument, aBlock, until it ends. aBlock 	MUST NOT contain an '^'. Evaluate block2 with the current context 	prior to each instruction executed. Answer the simulated value of aBlock."	| current |	aBlock hasMethodReturn		ifTrue: [self error: 'simulation of blocks with ^ can run loose'].	current := aBlock asContext.	current pushArgs: Array new from: self.	[current == self]		whileFalse:			[block2 value: current.			current := current step].	^self pop! !