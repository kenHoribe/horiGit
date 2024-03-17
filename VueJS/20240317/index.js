/*
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
