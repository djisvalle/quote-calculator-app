import axios from "axios";

export const http = axios.create({ 
    baseURL: 'http://localhost:5271/api',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    }
});