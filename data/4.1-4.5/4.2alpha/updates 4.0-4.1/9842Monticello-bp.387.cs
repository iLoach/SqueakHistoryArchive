"Change Set:		9842Monticello-bp.387Monticello-bp.387:fix height on button rows in Monticello toolsMonticello-nice.386:Fix http://bugs.squeak.org/view.php?id=74871) The package-cache did store as nil2) The file was not closed"!!MCWorkingCopyBrowser methodsFor: 'actions' stamp: 'nice 3/31/2010 22:30'!saveRepositories	FileStream forceNewFileNamed: 'MCRepositories.st' do: [:f |		MCRepositoryGroup default repositoriesDo: [:r |			r asCreationTemplate ifNotNil: [:template |				f nextPutAll: 'MCRepositoryGroup default addRepository: (', template , ')!!'; cr]]]! !!MCTool methodsFor: 'morphic ui' stamp: 'bp 4/2/2010 21:35'!buildWith: builder	|  windowBuilder |	windowBuilder := MCToolWindowBuilder builder: builder tool: self.	self widgetSpecs do:		[:spec | | send fractions offsets |		send := spec first.		fractions := spec at: 2 ifAbsent: [#(0 0 1 1)].		offsets := spec at: 3 ifAbsent: [#(0 0 0 0)].		windowBuilder frame: (LayoutFrame			fractions: (fractions first @ fractions second corner: fractions third @ fractions fourth)			offsets: (offsets first @ offsets second corner: offsets third @ offsets fourth)).		windowBuilder perform: send first withArguments: send allButFirst].	^ windowBuilder build! !