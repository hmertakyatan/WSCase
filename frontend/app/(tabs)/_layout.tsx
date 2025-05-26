import { Ionicons } from '@expo/vector-icons';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Tabs, useRouter } from 'expo-router';
import React, { useEffect, useState } from 'react';

const _Layout = () => {
  const router = useRouter();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const checkToken = async () => {
      const token = await AsyncStorage.getItem('jwt');

      if (!token) {
        setLoading(false);      
        router.replace('/login'); 
      } else {
        setLoading(false);    
      }
    };

    checkToken();
  }, []);

  if (loading) {

    return null;
  }

  return (
    <Tabs
      screenOptions={({ route }) => {
        let iconName: keyof typeof Ionicons.glyphMap | undefined;

        if (route.name === 'index') {
          iconName = 'home';
        } else if (route.name === 'Products') {
          iconName = 'list';
        } else if (route.name === 'Profile') {
          iconName = 'person';
        } else {
          iconName = 'help';
        }

        return {
          tabBarIcon: ({ color, size }) =>
            iconName ? (
              <Ionicons name={iconName} size={size} color={color} />
            ) : null,
          tabBarActiveTintColor: '#007AFF',
          tabBarInactiveTintColor: 'gray',
          headerShown: false,
        };
      }}
    >
      <Tabs.Screen name="index" options={{ title: 'Anasayfa' }} />
      <Tabs.Screen name="Products" options={{ title: 'Ürünler' }} />
      <Tabs.Screen name="Profile" options={{ title: 'Profil' }} />
    </Tabs>
  );
};

export default _Layout;
