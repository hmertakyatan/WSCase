import AsyncStorage from '@react-native-async-storage/async-storage';
import { useRouter } from 'expo-router';
import { useEffect } from 'react';
import { StyleSheet, Text, View } from 'react-native';

export default function Splash() {
  const router = useRouter();

  useEffect(() => {
    const checkAuth = async () => {
      await new Promise((res) => setTimeout(res, 4000));

      const token = await AsyncStorage.getItem('token');
      if (token) {
        router.replace('/');
      } else {
        router.replace('/login'); 
      }
    };
    
    checkAuth();
  }, []);

  return (
    <View style={styles.container}>
      <Text style={styles.text}>Hoşgeldiniz</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  text: { fontSize: 32, fontWeight: 'bold' },
});