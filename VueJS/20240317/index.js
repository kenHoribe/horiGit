/* Vue2
new Vue ({
    el:'#ex1',
    data: {
        luffy: 'ルフィ'
    },
});
*/

//Vue3
Vue.createApp({
    data: function(){
        return {
            luffy: "ルフィ"
        };
    }
}).mount("#ex1");

Vue.createApp({
    data: function(){
        return {
            luffy: "ルフィ"
        };
    }
}).mount("#ex2");

Vue.createApp({
    data: function(){
        return {
            luffy: "ルフィ"
        };
    }
}).mount("#ex3");

Vue.createApp({
    data: function(){
        return {
            unkoUrl: "https://one-piece.com/"
        };
    }
}).mount("#ex4");

Vue.createApp({ 
    data:function(){
        return {
            flag: true
        };
    }
}).mount("#ex5");

Vue.createApp({
    data(){
        return {
            characters:[
                {
                    name: "ベポ",
                    bounty: 500
                },
                {
                    name: "チョッパー",
                    bounty: 1000
                },
            ]
        };
    },
    computed: {
        fiterbounty(){
            return this.characters.filter(c=>c.bounty >= 800);
        }
    }
}).mount("#ex9");
