import React, { useEffect, useState } from 'react';
import {
  ActivityIndicator,
  Alert,
  Button,
  Dimensions,
  FlatList,
  Image,
  StyleSheet,
  Text,
  TouchableOpacity,
  View,
} from 'react-native';
import apiClient, { STATIC_BASE_URL } from '../api/api';

type Category = {
  id: string;
  categoryName: string;
  categoryIcon: string;
};

type CategoryResponse = {
  data: Category[];
  message: string;
  success: boolean;
};

interface Product {
  id: string;
  productName: string;
  productIcon: string;
}

type ProductResponse = {
  data: Product[];
  message: string;
  success: boolean;
};

const Products: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [selectedCategories, setSelectedCategories] = useState<string[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [loadingCategories, setLoadingCategories] = useState<boolean>(true);
  const [loadingProducts, setLoadingProducts] = useState<boolean>(false);

  const screenWidth = Dimensions.get('window').width;
  const itemMargin = 10;
  const itemsPerRow = 2;
  const itemWidth = (screenWidth - (itemsPerRow + 1) * itemMargin) / itemsPerRow;

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        setLoadingCategories(true);
        const response = await apiClient.get<CategoryResponse>('/category');
        setCategories(response.data.data);
      } catch (error) {
        Alert.alert('Hata', 'Kategoriler alınırken hata oluştu.');
        console.error(error);
      } finally {
        setLoadingCategories(false);
      }
    };

    fetchCategories();
  }, []);

  const toggleCategory = (id: string) => {
    setSelectedCategories(prev =>
      prev.includes(id) ? prev.filter(catId => catId !== id) : [...prev, id]
    );
  };

  const handleFetchProducts = async () => {
    if (selectedCategories.length === 0) {
      Alert.alert('Hata', 'Lütfen en az bir kategori seçin!');
      return;
    }

     try {
      setLoadingProducts(true);
      const response = await apiClient.post<ProductResponse>('/product/by-categories', selectedCategories);
      setProducts(response.data.data);
    } catch (error) {
      Alert.alert('Hata', 'Ürünler alınırken bir hata oluştu.');
      console.error(error);
    } finally {
      setLoadingProducts(false);
  }
  };

  if (loadingCategories) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
        <Text>Kategoriler yükleniyor...</Text>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Kategoriler:</Text>

      <View style={styles.categoryContainer}>
        {categories.map(cat => {
          const selected = selectedCategories.includes(cat.id);
          return (
            <TouchableOpacity
              key={cat.id}
              style={[
                styles.categoryItem,
                selected && styles.categoryItemSelected,
              ]}
              onPress={() => toggleCategory(cat.id)}
            >
              <Image
                source={{ uri: `${STATIC_BASE_URL}${cat.categoryIcon}` }}
                style={styles.categoryIcon}
                resizeMode="contain"
              />
              <Text
                style={[
                  styles.categoryText,
                  selected && styles.categoryTextSelected,
                ]}
              >
                {cat.categoryName}
              </Text>
            </TouchableOpacity>
          );
        })}
      </View>

      <View style={styles.buttonWrapper}>
        <Button
          title={loadingProducts ? 'Yükleniyor...' : 'Ürünleri Getir'}
          onPress={handleFetchProducts}
          disabled={loadingProducts}
        />
      </View>

      <Text style={styles.title}>Ürünler:</Text>
      {products.length === 0 ? (
        <Text>Henüz ürün yok</Text>
      ) : (
        <FlatList
          data={products}
          numColumns={itemsPerRow}
          keyExtractor={item => item.id}
          columnWrapperStyle={{ justifyContent: 'flex-start' }}
          renderItem={({ item }) => (
            <View style={[styles.productCard, { width: itemWidth, margin: itemMargin / 2 }]}>
              <Image
                source={{ uri: `${STATIC_BASE_URL}${item.productIcon}` }}
                style={styles.productIcon}
                resizeMode="cover"
              />
              <View style={styles.productInfo}>
                <Text style={styles.productName}>{item.productName}</Text>
              </View>
            </View>
          )}
        />
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: { padding: 20, flex: 1 },
  center: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  title: { fontWeight: 'bold', fontSize: 18, marginVertical: 10 },

  categoryContainer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 10,
    marginBottom: 20,
  },
  categoryItem: {
    flexDirection: 'column',
    alignItems: 'center',
    backgroundColor: '#f2f2f2',
    borderRadius: 10,
    padding: 10,
    width: 100,
  },
  categoryItemSelected: {
    backgroundColor: '#cce5ff',
  },
  categoryIcon: {
    width: 30,
    height: 30,
    marginBottom: 5,
  },
  categoryText: {
    textAlign: 'center',
    color: 'black',
  },
  categoryTextSelected: {
    color: '#0056b3',
    fontWeight: 'bold',
  },

  buttonWrapper: {
    alignItems: 'center',
    marginVertical: 10,
    width: '100%',
  },

  productCard: {
    backgroundColor: '#fff',
    borderRadius: 10,
    padding: 10,
    overflow: 'hidden',
    alignItems: 'center',
  },
  productIcon: {
    width: '50%',
    aspectRatio: 1,
    borderRadius: 8,
    backgroundColor: '#eee',
    marginBottom: 10,
  },
  productInfo: {
    alignItems: 'center',
  },
  productName: {
    fontSize: 16,
    fontWeight: 'bold',
    textAlign: 'center',
  },
});

export default Products;
