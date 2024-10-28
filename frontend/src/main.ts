import { createApp } from 'vue'
import Antd from 'ant-design-vue'
import './style.css'
import App from './App.vue'
import axios from 'axios'
import 'ant-design-vue/dist/reset.css'


axios.defaults.baseURL = 'http://localhost:5026';

createApp(App)
    .use(Antd)
    .mount('#app')
