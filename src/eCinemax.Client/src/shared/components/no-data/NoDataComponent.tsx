import { View, Text } from "react-native";
import React from "react";

interface Props {
  text?: string;
}

const NoDataComponent = (props: Props) => {
  const { text } = props;
  return (
    <View>
      <Text className="text-white text-center mt-8">
        {text ?? "Không có dữ liệu"}
      </Text>
    </View>
  );
};

export default NoDataComponent;
