import AsyncStorage from '@react-native-async-storage/async-storage';
import axios from 'axios';
import { Alert } from 'react-native';
import { logout } from './logout';
import { router } from 'expo-router';

export const API_BASE_URL = 'http://192.168.1.33:5270/api';
export const STATIC_BASE_URL = 'http://192.168.1.33:5270/';


const apiClient = axios.create({
  baseURL: API_BASE_URL,
});

apiClient.interceptors.request.use(
  async config => {
    const token = await AsyncStorage.getItem('jwt');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  error => Promise.reject(error)
);
apiClient.interceptors.response.use(
  response => response,
  async error => {
    if (error.response?.status === 401) {
      await logout();
      router.replace('/login');

      Alert.alert('Oturum Süresi Doldu', 'Lütfen tekrar giriş yapınız.');
    }
    return Promise.reject(error);
  }
);

export default apiClient;