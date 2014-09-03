// ----------------------------------------------------------------------------
// markItUp!
// ----------------------------------------------------------------------------
// Copyright (C) 2008 Jay Salvat
// http://markitup.jaysalvat.com/
// ----------------------------------------------------------------------------
// BBCode tags example
// http://en.wikipedia.org/wiki/Bbcode
// ----------------------------------------------------------------------------
// Feel free to add more tags
// ----------------------------------------------------------------------------
mySettings = {
	previewParserPath: '', // path to your BBCode parser
	markupSet: [
		{ name: 'Bold', key: 'B', openWith: '[b]', closeWith: '[/b]' },
		{ name: 'Italic', key: 'I', openWith: '[i]', closeWith: '[/i]' },
		{ name: 'Underline', key: 'U', openWith: '[u]', closeWith: '[/u]' },
		{ separator: '---------------' },
		{ name: 'Picture', key: 'P', replaceWith: '[img][![Url]!][/img]' },
		{ name: 'Link', key: 'L', openWith: '[url=[![Url]!]]', closeWith: '[/url]', placeHolder: 'Your text to link here...' },
		{ separator: '---------------' },
		{
			name: 'Size', key: 'S', openWith: '[size=[![Text size]!]]', closeWith: '[/size]',
			dropMenu: [
				{ name: 'Big', openWith: '[size=200]', closeWith: '[/size]' },
				{ name: 'Normal', openWith: '[size=100]', closeWith: '[/size]' },
				{ name: 'Small', openWith: '[size=50]', closeWith: '[/size]' }
			]
		},
		{	name:'Colors', className:'palette', dropMenu: [
				{name:'Yellow',	replaceWith:'#FCE94F',	className:"col1-1" },
				{name:'Yellow',	replaceWith:'#EDD400', 	className:"col1-2" },
				{name:'Yellow', replaceWith:'#C4A000', 	className:"col1-3" },
				
				{name:'Orange', replaceWith:'#FCAF3E', 	className:"col2-1" },
				{name:'Orange', replaceWith:'#F57900', 	className:"col2-2" },
				{name:'Orange', replaceWith:'#CE5C00', 	className:"col2-3" },
				
				{name:'Brown', 	replaceWith:'#E9B96E', 	className:"col3-1" },
				{name:'Brown', 	replaceWith:'#C17D11', 	className:"col3-2" },
				{name:'Brown',	replaceWith:'#8F5902', 	className:"col3-3" },
				
				{name:'Green', 	replaceWith:'#8AE234', 	className:"col4-1" },
				{name:'Green', 	replaceWith:'#73D216', 	className:"col4-2" },
				{name:'Green',	replaceWith:'#4E9A06', 	className:"col4-3" },
				
				{name:'Blue', 	replaceWith:'#729FCF', 	className:"col5-1" },
				{name:'Blue', 	replaceWith:'#3465A4', 	className:"col5-2" },
				{name:'Blue',	replaceWith:'#204A87', 	className:"col5-3" },
	
				{name:'Purple', replaceWith:'#AD7FA8', 	className:"col6-1" },
				{name:'Purple', replaceWith:'#75507B', 	className:"col6-2" },
				{name:'Purple',	replaceWith:'#5C3566', 	className:"col6-3" },
				
				{name:'Red', 	replaceWith:'#EF2929', 	className:"col7-1" },
				{name:'Red', 	replaceWith:'#CC0000', 	className:"col7-2" },
				{name:'Red',	replaceWith:'#A40000', 	className:"col7-3" },
				
				{name:'Gray', 	replaceWith:'#FFFFFF', 	className:"col8-1" },
				{name:'Gray', 	replaceWith:'#D3D7CF', 	className:"col8-2" },
				{name:'Gray',	replaceWith:'#BABDB6', 	className:"col8-3" },
				
				{name:'Gray', 	replaceWith:'#888A85', 	className:"col9-1" },
				{name:'Gray', 	replaceWith:'#555753', 	className:"col9-2" },
				{name:'Gray',	replaceWith:'#000000', 	className:"col9-3" }
		]
		},
		{ separator: '---------------' },
		{ name: 'Bulleted list', openWith: '[list]\n', closeWith: '\n[/list]' },
		{ name: 'Numeric list', openWith: '[list=[![Starting number]!]]\n', closeWith: '\n[/list]' },
		{ name: 'List item', openWith: '[*] ' },
		{ separator: '---------------' },
		{ name: 'Quotes', openWith: '[quote]', closeWith: '[/quote]' },
		{ name: 'Code', openWith: '[code]', closeWith: '[/code]' }
	]
}