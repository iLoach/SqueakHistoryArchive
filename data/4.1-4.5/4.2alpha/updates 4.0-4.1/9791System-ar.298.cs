"Change Set:		9791System-ar.298System-ar.298:Updated default window color preferences."!!Preferences class methodsFor: 'standard queries' stamp: ''!lexiconWindowColor	^ self		valueOfFlag: #lexiconWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!instanceBrowserWindowColor	^ self		valueOfFlag: #instanceBrowserWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!fileListWindowColor	^ self		valueOfFlag: #fileListWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!debuggerWindowColor	^ self		valueOfFlag: #debuggerWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!defaultWindowColor	^ self		valueOfFlag: #defaultWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!workspaceWindowColor	^ self		valueOfFlag: #workspaceWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'window colors' stamp: 'tfel 2/27/2010 19:34'!windowColorHelp	"Provide help for the window-color panel"	| helpString |	helpString := 'The "Window Colors" panel lets you select colors for many kinds of standard Squeak windows.You can change your color preference for any particular tool by clicking on the color swatch and then selecting the desired color from the resulting color-picker.The three buttons entitled "Bright", "Pastel", and "Gray" let you revert to any of three different standard color schemes.  The choices you make in the Window Colors panel only affect the colors of new windows that you open.You can make other tools have their colors governed by this panel by simply implementing #windowColorSpecification on the class side of the model -- consult implementors of that method to see examples of how to do this.'.	 (StringHolder new contents: helpString)		openLabel: 'About Window Colors'	"Preferences windowColorHelp"! !!Preferences class methodsFor: 'standard queries' stamp: ''!classCommentVersionsBrowserWindowColor	^ self		valueOfFlag: #classCommentVersionsBrowserWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!fileContentsBrowserWindowColor	^ self		valueOfFlag: #fileContentsBrowserWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!versionsBrowserWindowColor	^ self		valueOfFlag: #versionsBrowserWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!testRunnerWindowColor	^ self		valueOfFlag: #testRunnerWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!dualChangeSorterWindowColor	^ self		valueOfFlag: #dualChangeSorterWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!messageListWindowColor	^ self		valueOfFlag: #messageListWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!messageNamesWindowColor	^ self		valueOfFlag: #messageNamesWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!browserWindowColor	^ self		valueOfFlag: #browserWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!packageLoaderWindowColor	^ self		valueOfFlag: #packageLoaderWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!transcriptWindowColor	^ self		valueOfFlag: #transcriptWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'window colors' stamp: 'ar 3/24/2010 21:05'!installWindowColorsVia: colorSpecBlock	"Install windows colors using colorSpecBlock to deliver the color source for each element; the block is handed a WindowColorSpec object"	"Preferences installBrightWindowColors"		WindowColorRegistry refresh.	self windowColorTable do:		[:aColorSpec | | color |			color := (Color colorFrom: (colorSpecBlock value: aColorSpec)).			self setWindowColorFor: aColorSpec classSymbol to: color].	SystemWindow withAllSubclasses do: [:c | 		c allInstances do: [:w | w refreshWindowColor]].! !!Preferences class methodsFor: 'standard queries' stamp: ''!changeSorterWindowColor	^ self		valueOfFlag: #changeSorterWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!methodFinderWindowColor	^ self		valueOfFlag: #methodFinderWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!changeListWindowColor	^ self		valueOfFlag: #changeListWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!preferenceBrowserWindowColor	^ self		valueOfFlag: #preferenceBrowserWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'standard queries' stamp: ''!packageBrowserWindowColor	^ self		valueOfFlag: #packageBrowserWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !!Preferences class methodsFor: 'window colors' stamp: 'tfel 2/28/2010 16:54'!installUniformWindowColors	"Install the factory-provided uniform window colors for all tools"	"Preferences installUniformWindowColors"	self installWindowColorsVia: [:aQuad | Color veryVeryLightGray muchLighter]! !!Preferences class methodsFor: 'standard queries' stamp: ''!monticelloToolWindowColor	^ self		valueOfFlag: #monticelloToolWindowColor		ifAbsent: [Color				r: 0.971				g: 0.971				b: 0.971]! !Preferences class removeSelector: #clickOnLabelToEdit!