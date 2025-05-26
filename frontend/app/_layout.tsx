import { Stack } from "expo-router";

export default function RootLayout() {
   return (
     <Stack 
      initialRouteName="splash"
      screenOptions={{ headerShown: false }}>
      <Stack.Screen name="splash" />
      <Stack.Screen name="login" />
      <Stack.Screen name="(tabs)" />
    </Stack>
  );
}
