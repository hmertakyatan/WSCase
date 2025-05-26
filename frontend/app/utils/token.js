import AsyncStorage from '@react-native-async-storage/async-storage';

export const saveToken = async (token) => {
  try {
    await AsyncStorage.setItem('jwt', token);
  } catch (e) {
    console.log('Token kaydedilemedi:', e);
  }
};

export const getToken = async () => {
  try {
    return await AsyncStorage.getItem('jwt');
  } catch (e) {
    console.log('Token okunamadı:', e);
  }
};

export const removeToken = async () => {
  try {
    await AsyncStorage.removeItem('jwt');
  } catch (e) {
    console.log('Token silinemedi:', e);
  }
};
