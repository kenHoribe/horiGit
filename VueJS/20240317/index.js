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
