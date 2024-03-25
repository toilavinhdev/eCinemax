import { StyleSheet, Text, View } from "react-native";
import { EMovieStatus } from "~/features/movie/movie.interfaces";
import { colors } from "~/shared/constants";
import React from "react";

interface Props {
  currentStatus: EMovieStatus;
  setStatus: (status: EMovieStatus) => void;
}

const MovieStatusComponent = (props: Props) => {
  const { currentStatus, setStatus } = props;
  return (
    <View className="flex-row justify-center mt-2">
      <Text
        onPress={() => setStatus(EMovieStatus.NowShowing)}
        style={{
          ...styles.textStatus,
          color:
            currentStatus === EMovieStatus.NowShowing
              ? colors.primary
              : "white",
        }}
      >
        Now Showing
      </Text>
      <Text
        onPress={() => setStatus(EMovieStatus.ComingSoon)}
        style={{
          ...styles.textStatus,
          color:
            currentStatus === EMovieStatus.ComingSoon
              ? colors.primary
              : "white",
        }}
      >
        Comming Soon
      </Text>
    </View>
  );
};

const styles = StyleSheet.create({
  textStatus: {
    color: colors.primary,
    fontSize: 18,
    flex: 1,
    textAlign: "center",
    padding: 12,
  },
});

export default MovieStatusComponent;
