/*
new Vue ({
    el:'#app',
    data: {
        luffy: 'ルフィ'
    },
});
*/

Vue.createApp({
    data: function(){
        return {
            luffy:"ルフィ"
        };
    }
}).mount("#ex1");