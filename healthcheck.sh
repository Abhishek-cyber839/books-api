declare -a urls=("https://stackoverflow.com/questions/8880603/loop-through-an-array-of-strings-in-bash" "https://stackoverflow.com/questions/8880adasl" "https://stackoverflow.com/questis/" "https://stackoverflow.com/questions/-88806012/" "https://stackoverflow.com/questions/8880603/lasdasd")
# replace the above testing urls with the healthcheck endpoints of the book-store-api dotnet app, to check if the api is healthy or not
## now loop through the above array
# get length of an array
arraylength=${#urls[@]}

# use for loop to read all values and indexes
for (( i=0; i<${arraylength}; i++ ));
do
  echo "sending request for index: $i, value: ${urls[$i]}"
  http_response=$(curl -s -o dev/null -w "%{http_code}" ${urls[$i]})
  if [ $http_response != "200" ]; then
      echo "request failed with status code : $http_response"
  else
      echo "request succeeded with status code : $http_response"
  fi
done
# You can access them using echo "${arr[0]}", "${arr[1]}" also
