import apiClient from './api';

export const loginUser = async (username, password) => {
  try {
    const response = await apiClient.post('/auth/login', { username, password });
    console.log(response.data)
    return response.data.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || 'Giriş başarısız');
  }
};
