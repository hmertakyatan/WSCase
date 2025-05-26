import AsyncStorage from '@react-native-async-storage/async-storage';
import { useRouter } from 'expo-router';
import { useEffect, useState } from 'react';
import { Alert, Button, Dimensions, StyleSheet, Text, TextInput, View } from 'react-native';
import { loginUser } from './api/login';
import { saveToken } from './utils/token';

const { height, width } = Dimensions.get('window');

const LoginScreen = () => {
  const router = useRouter();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  useEffect(() => {
    const checkToken = async () => {
      const token = await AsyncStorage.getItem('jwt');
      if (token) {
        router.replace('/');
      }

    };
    checkToken();
  }, []);

  const handleLogin = async () => {
    try {
      const token = await loginUser(username, password);
      await saveToken(token);
      console.log(token);
      Alert.alert('Giriş Başarılı');
      router.replace('/');
    } catch (error) {
      Alert.alert('Hata', error.message);
    }
  };

   return (
    <View style={styles.container}>
      <View style={styles.formBox}>
        <Text style={styles.title}>Giriş</Text>

        <View style={styles.formContent}>
          <TextInput
            placeholder="Kullanıcı Adı"
            value={username}
            onChangeText={setUsername}
            style={styles.input}
            autoCapitalize="none"
          />
          <TextInput
            placeholder="Şifre"
            value={password}
            onChangeText={setPassword}
            secureTextEntry
            style={styles.input}
          />
          <Button title="Giriş Yap" onPress={handleLogin} />
        </View>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#f2f2f2',
  },
  formBox: {
    width: width * 0.85,
    height: height * 0.40,
    backgroundColor: '#fff',
    borderRadius: 12,
    padding: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.3,
    shadowRadius: 5,
    elevation: 8,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 20,
  },
  formContent: {
    gap: 50,
  },
  input: {
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 8,
    padding: 12,
    fontSize: 16,
  },
});

export default LoginScreen;