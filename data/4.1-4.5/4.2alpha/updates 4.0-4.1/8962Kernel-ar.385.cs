"Change Set:		8962Kernel-ar.385Kernel-ar.385:Fix handling of Class>>binding:- Verify that the registered class in the environment is the receiver- Install a common binding after recompilation - Add an accessor for installing the binding in CompiledMethod"!!Behavior methodsFor: 'compiling' stamp: 'ar 2/2/2010 10:02'!compileAllFrom: oldClass	"Compile all the methods in the receiver's method dictionary.	This validates sourceCode and variable references and forces	all methods to use the current bytecode set"	| binding |	"ar 7/10/1999: Use oldClass selectors not self selectors"	oldClass selectorsDo: [:sel | self recompile: sel from: oldClass].		"Ensure that we share a common binding after recompilation.	This is so that ClassBuilder reshapes avoid creating new bindings	for every method when recompiling a large class hierarchy."	binding := self binding.	self methodsDo:[:m|		m methodClassAssociation == binding 			ifFalse:[m methodClassAssociation: binding]].! !!CompiledMethod methodsFor: 'accessing' stamp: 'ar 2/2/2010 09:57'!methodClassAssociation: aBinding	"sets the association to the class that I am installed in"	^self literalAt: self numLiterals put: aBinding! !!Class methodsFor: 'compiling' stamp: 'ar 2/2/2010 09:53'!binding	"Answer a binding for the receiver, sharing if possible"	| binding |	binding := Smalltalk associationAt: name ifAbsent: [nil -> self].	^binding value == self ifTrue:[binding] ifFalse:[nil -> self].! !