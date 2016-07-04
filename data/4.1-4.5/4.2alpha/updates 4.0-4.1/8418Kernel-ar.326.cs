"Change Set:		8418Kernel-ar.326Kernel-ar.326:Processor anyProcessesAbove: should consult allSubInstances of Process."!!ProcessorScheduler methodsFor: 'private' stamp: 'ar 12/10/2009 00:22'!anyProcessesAbove: highestPriority 	"Do any instances of Process exist with higher priorities?"	^(Process allSubInstances select: [:aProcess | 		aProcess priority > highestPriority]) isEmpty		"If anyone ever makes a subclass of Process, be sure to use allSubInstances."! !