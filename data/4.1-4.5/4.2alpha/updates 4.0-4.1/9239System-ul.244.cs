"Change Set:		9239System-ul.244System-ul.244:- don't use asSortedCollection for sorting- minor cleanup- add missing method: PseudoClass >> #selectorsDo: (probably there are more missing methods)"!!MessageTally methodsFor: 'printing' stamp: 'ul 2/4/2010 15:34'!leavesPrintExactOn: aStream	| dict |	dict := IdentityDictionary new: 100.	self leavesInto: dict fromSender: nil.	dict values sort		do: [ :node |			node printOn: aStream total: tally totalTime: nil tallyExact: true.			node printSenderCountsOn: aStream ]! !!PseudoClass methodsFor: 'methods' stamp: 'ul 2/5/2010 06:34'!selectorsDo: aBlock	^self sourceCode keysDo: aBlock! !!MessageTally methodsFor: 'printing' stamp: 'ul 2/5/2010 21:39'!rootPrintOn: aStream total: total totalTime: totalTime threshold: threshold	| groups sons |	sons := self sonsOver: threshold.	groups := sons groupBy: [ :aTally | aTally process] having: [ :g | true].	groups keysAndValuesDo: [ :p :g |		(reportOtherProcesses or: [ p notNil ]) ifTrue: [			aStream nextPutAll: '--------------------------------'; cr.			aStream nextPutAll: 'Process: ',  (p ifNil: [ 'other processes'] ifNotNil: [ p browserPrintString]); cr.			aStream nextPutAll: '--------------------------------'; cr.			g sort do:[:aSon | 				aSon 					treePrintOn: aStream					tabs: OrderedCollection new					thisTab: ''					total: total					totalTime: totalTime					tallyExact: false					orThreshold: threshold]].	]! !!MessageTally methodsFor: 'printing' stamp: 'ul 2/5/2010 21:45'!printSenderCountsOn: aStream	| mergedSenders |	mergedSenders := IdentityDictionary new.	senders do:		[:node |		| mergedNode |		mergedNode := mergedSenders at: node method ifAbsent: [nil].		mergedNode == nil			ifTrue: [mergedSenders at: node method put: node]			ifFalse: [mergedNode bump: node tally]].	mergedSenders values sort do:		[:node | 		10 to: node tally printString size by: -1 do: [:i | aStream space].		node printOn: aStream total: tally totalTime: nil tallyExact: true]! !!MessageTally methodsFor: 'printing' stamp: 'ul 2/5/2010 21:48'!treePrintOn: aStream tabs: tabs thisTab: myTab total: total totalTime: totalTime tallyExact: isExact orThreshold: threshold 	| sons |	tabs do: [:tab | aStream nextPutAll: tab].	tabs size > 0 		ifTrue: 			[self 				printOn: aStream				total: total				totalTime: totalTime				tallyExact: isExact].	sons := isExact ifTrue: [receivers] ifFalse: [self sonsOver: threshold].	sons isEmpty 		ifFalse: 			[tabs addLast: myTab.			sons sort.			1 to: sons size do: [ :i | 				| sonTab |				sonTab := i < sons size ifTrue: ['  |'] ifFalse: ['  '].				(sons at: i) 					treePrintOn: aStream					tabs: (tabs size < self maxTabs 							ifTrue: [tabs]							ifFalse: [(tabs select: [:x | x = '[']) copyWith: '['])					thisTab: sonTab					total: total					totalTime: totalTime					tallyExact: isExact					orThreshold: threshold].			tabs removeLast]! !!MessageTally methodsFor: 'printing' stamp: 'ul 2/4/2010 15:34'!leavesPrintOn: aStream threshold: threshold	| dict |	dict := IdentityDictionary new: 100.	self leavesInto: dict fromSender: nil.	(dict values select: [:node | node tally > threshold])		sort do: [:node |			node printOn: aStream total: tally totalTime: time tallyExact: false ]! !