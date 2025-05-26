import AsyncStorage from '@react-native-async-storage/async-storage';
import { useRouter } from 'expo-router';
import React from 'react';
import { Button, Text, View } from 'react-native';


const Profile = () => {
  const router = useRouter();
  const handleLogout = async () => {
    await AsyncStorage.removeItem('jwt');
    router.replace('/login');
    alert('Başarıyla çıkış yapıldı!');
  };

  return (
    <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
      <Text>Profile</Text>
      <Button title="Çıkış Yap" onPress={handleLogout} />
    </View>
  );
};

export default Profile;
