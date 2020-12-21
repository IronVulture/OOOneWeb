
var camelCaseTokenizer = function (builder) {

  var pipelineFunction = function (token) {
    var previous = '';
    // split camelCaseString to on each word and combined words
    // e.g. camelCaseTokenizer -> ['camel', 'case', 'camelcase', 'tokenizer', 'camelcasetokenizer']
    var tokenStrings = token.toString().trim().split(/[\s\-]+|(?=[A-Z])/).reduce(function(acc, cur) {
      var current = cur.toLowerCase();
      if (acc.length === 0) {
        previous = current;
        return acc.concat(current);
      }
      previous = previous.concat(current);
      return acc.concat([current, previous]);
    }, []);

    // return token for each string
    // will copy any metadata on input token
    return tokenStrings.map(function(tokenString) {
      return token.clone(function(str) {
        return tokenString;
      })
    });
  }

  lunr.Pipeline.registerFunction(pipelineFunction, 'camelCaseTokenizer')

  builder.pipeline.before(lunr.stemmer, pipelineFunction)
}
var searchModule = function() {
    var documents = [];
    var idMap = [];
    function a(a,b) { 
        documents.push(a);
        idMap.push(b); 
    }

    a(
        {
            id:0,
            title:"TextureColor",
            content:"TextureColor",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOneUnityTools.Editor/TextureColor',
            title:"TextureColor",
            description:""
        }
    );
    a(
        {
            id:1,
            title:"UnityPathUtilityTests",
            content:"UnityPathUtilityTests",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOneUnityTools.Editor.Tests/UnityPathUtilityTests',
            title:"UnityPathUtilityTests",
            description:""
        }
    );
    a(
        {
            id:2,
            title:"CSharpFileUtilityTests",
            content:"CSharpFileUtilityTests",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOneUnityTools.Editor.Tests/CSharpFileUtilityTests',
            title:"CSharpFileUtilityTests",
            description:""
        }
    );
    a(
        {
            id:3,
            title:"UnityFileUtilityTests",
            content:"UnityFileUtilityTests",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOneUnityTools.Editor.Tests/UnityFileUtilityTests',
            title:"UnityFileUtilityTests",
            description:""
        }
    );
    a(
        {
            id:4,
            title:"SecTextureData",
            content:"SecTextureData",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOneUnityTools.Editor/SecTextureData',
            title:"SecTextureData",
            description:""
        }
    );
    a(
        {
            id:5,
            title:"UnityFileUtility FileType",
            content:"UnityFileUtility FileType",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOneUnityTools.Editor/FileType',
            title:"UnityFileUtility.FileType",
            description:""
        }
    );
    a(
        {
            id:6,
            title:"UnityPathUtility",
            content:"UnityPathUtility",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOneUnityTools.Editor/UnityPathUtility',
            title:"UnityPathUtility",
            description:""
        }
    );
    a(
        {
            id:7,
            title:"CSharpFileUtility",
            content:"CSharpFileUtility",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOneUnityTools.Editor/CSharpFileUtility',
            title:"CSharpFileUtility",
            description:""
        }
    );
    a(
        {
            id:8,
            title:"UnityFileUtility",
            content:"UnityFileUtility",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOneUnityTools.Editor/UnityFileUtility',
            title:"UnityFileUtility",
            description:""
        }
    );
    a(
        {
            id:9,
            title:"TestRunnerEditor",
            content:"TestRunnerEditor",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOne.Tools.Editor/TestRunnerEditor',
            title:"TestRunnerEditor",
            description:""
        }
    );
    a(
        {
            id:10,
            title:"AutoOrderInLayer",
            content:"AutoOrderInLayer",
            description:'',
            tags:''
        },
        {
            url:'/OOOneWeb/api/OOOneUnityTools/AutoOrderInLayer',
            title:"AutoOrderInLayer",
            description:""
        }
    );
    var idx = lunr(function() {
        this.field('title');
        this.field('content');
        this.field('description');
        this.field('tags');
        this.ref('id');
        this.use(camelCaseTokenizer);

        this.pipeline.remove(lunr.stopWordFilter);
        this.pipeline.remove(lunr.stemmer);
        documents.forEach(function (doc) { this.add(doc) }, this)
    });

    return {
        search: function(q) {
            return idx.search(q).map(function(i) {
                return idMap[i.ref];
            });
        }
    };
}();
