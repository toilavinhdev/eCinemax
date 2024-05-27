import {
  LocationObject,
  getCurrentPositionAsync,
  requestForegroundPermissionsAsync,
} from "expo-location";
import React, { useEffect, useRef, useState } from "react";
import { Image, Text, View } from "react-native";
import MapView, { Marker, PROVIDER_GOOGLE } from "react-native-maps";
import { listCinemaAPI } from "~/features/cinema/cinema.apis";
import { ICinemaViewModel } from "~/features/cinema/cinema.interfaces";
import MapViewStyle from "~/shared/utils/map-view-style.json";

const CinemaScreen = () => {
  const [currentLocation, setCurrentLocation] = useState<LocationObject | null>(
    null
  );
  const [cinemas, setCinemas] = useState<ICinemaViewModel[]>([]);
  const mapRef = useRef<MapView>(null);

  const getCurrentLocation = async () => {
    const { status } = await requestForegroundPermissionsAsync();
    if (status !== "granted") {
      return;
    }
    let position = await getCurrentPositionAsync({});
    setCurrentLocation(position);
  };

  useEffect(() => {
    (async () => {
      const response = await listCinemaAPI();
      setCinemas(response.data.data);
    })();

    getCurrentLocation();
  }, []);

  return (
    currentLocation?.coords && (
      <View className="flex-1">
        <MapView
          showsUserLocation
          showsMyLocationButton
          ref={mapRef}
          provider={PROVIDER_GOOGLE}
          customMapStyle={MapViewStyle}
          region={{
            latitude: currentLocation.coords.latitude, // vĩ độ
            longitude: currentLocation.coords.longitude, // kinh độ
            latitudeDelta: 0.038,
            longitudeDelta: 0.038,
          }}
          className="w-full h-full"
        >
          {cinemas
            .filter((cinemas) => cinemas.location !== null)
            .map((cinema) => (
              <Marker
                key={cinema.id}
                coordinate={{
                  latitude: cinema.location!.latitude,
                  longitude: cinema.location!.longitude,
                }}
                title={cinema.name}
                description={cinema.address}
              ></Marker>
            ))}
        </MapView>
      </View>
    )
  );
};

export default CinemaScreen;
