using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.APItesting.mapAPI
{
    class Route
    {
        public bool hasTollRoad { get; set; }
        public List<int> computWaypoints { get; set; }
        public float fuelUsed { get; set; }
    }
}


/*
{
  "route": 
  {
    "hasTollRoad": false,
    "computedWaypoints": [],
    "fuelUsed": 0.27,
    "shape": 
    {
      "maneuverIndexes": [5],
      "shapePoints": [38.893165],
      "legIndexes": [189]
    },
    "hasUnpaved": false,
    "hasHighway": true,
    "realTime": 484,
    "boundingBox": 
    {
      "ul": 
      {
        "lng": -77.085411,
        "lat": 38.893276
      },
      "lr": 
      {
        "lng": -77.071159,
        "lat": 38.848869
      }
    },
    "distance": 4.573,
    "time": 445,
    "locationSequence": [0,1],
    "hasSeasonalClosure": false,
    "sessionId": "55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
    "locations": [
      {
        "latLng": 
        {
          "lng": -77.077959,
          "lat": 38.893165
        },
        "adminArea4": "Arlington",
        "adminArea5Type": "City",
        "adminArea4Type": "County",
        "adminArea5": "Arlington",
        "street": "[1700 - 1720] Clarendon Blvd",
        "adminArea1": "US",
        "adminArea3": "VA",
        "type": "s",
        "displayLatLng": 
        {
          "lng": -77.077957,
          "lat": 38.893165
        },
        "linkId": 13257655,
        "postalCode": "22209-2713",
        "sideOfStreet": "L",
        "dragPoint": false,
        "adminArea1Type": "Country",
        "geocodeQuality": "STREET",
        "geocodeQualityCode": "B1AAA",
        "adminArea3Type": "State"
      },
      {
        "latLng": {
          "lng": -77.081229,
          "lat": 38.848932
        },
        "adminArea4": "Arlington",
        "adminArea5Type": "City",
        "adminArea4Type": "County",
        "adminArea5": "Arlington",
        "street": "2400 S Glebe Rd",
        "adminArea1": "US",
        "adminArea3": "VA",
        "type": "s",
        "displayLatLng": {
          "lng": -77.08123,
          "lat": 38.84893
        },
        "linkId": 14874674,
        "postalCode": "22206-2500",
        "sideOfStreet": "R",
        "dragPoint": false,
        "adminArea1Type": "Country",
        "geocodeQuality": "POINT",
        "geocodeQualityCode": "P1AAA",
        "adminArea3Type": "State"
      }
    ],
    "hasCountryCross": false,
    "legs": [
      {
        "hasTollRoad": false,
        "index": 0,
        "roadGradeStrategy": [
          []
        ],
        "hasHighway": true,
        "hasUnpaved": false,
        "distance": 4.573,
        "time": 445,
        "origIndex": 3,
        "hasSeasonalClosure": false,
        "origNarrative": "Go south on Arlington Blvd/US-50 W.",
        "hasCountryCross": false,
        "formattedTime": "00:07:25",
        "destNarrative": "Proceed to 2400 S GLEBE RD.",
        "destIndex": 8,
        "maneuvers": [
          {
            "signs": [],
            "index": 0,
            "maneuverNotes": [],
            "direction": 8,
            "narrative": "Start out going east on Clarendon Blvd toward N Queen St.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/icon-dirs-start_sm.gif",
            "distance": 0.031,
            "time": 6,
            "linkIds": [],
            "streets": [
              "Clarendon Blvd"
            ],
            "attributes": 0,
            "transportMode": "AUTO",
            "formattedTime": "00:00:06",
            "directionName": "East",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-1,38.893164999999996,-77.077957,0,0|purple-2,38.893276,-77.077407,0,0|&center=38.8932205,-77.077682&zoom=15&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.077957,
              "lat": 38.893165
            },
            "turnType": 2
          },
          {
            "signs": [],
            "index": 1,
            "maneuverNotes": [],
            "direction": 4,
            "narrative": "Turn right onto N Queen St.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/rs_right_sm.gif",
            "distance": 0.168,
            "time": 32,
            "linkIds": [],
            "streets": [
              "N Queen St"
            ],
            "attributes": 0,
            "transportMode": "AUTO",
            "formattedTime": "00:00:32",
            "directionName": "South",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-2,38.893276,-77.077407,0,0|purple-3,38.890857,-77.07708699999999,0,0|&center=38.8920665,-77.077247&zoom=12&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.077407,
              "lat": 38.893276
            },
            "turnType": 2
          },
          {
            "signs": [],
            "index": 2,
            "maneuverNotes": [],
            "direction": 7,
            "narrative": "Turn right onto 14th St N.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/rs_right_sm.gif",
            "distance": 0.003,
            "time": 5,
            "linkIds": [],
            "streets": [
              "14th St N"
            ],
            "attributes": 0,
            "transportMode": "AUTO",
            "formattedTime": "00:00:05",
            "directionName": "West",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-3,38.890857,-77.07708699999999,0,0|purple-4,38.890842,-77.077148,0,0|&center=38.8908495,-77.07711749999999&zoom=15&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.077087,
              "lat": 38.890857
            },
            "turnType": 2
          },
          {
            "signs": [
              {
                "text": "50",
                "extraText": "",
                "direction": 7,
                "type": 2,
                "url": "http://icons.mqcdn.com/icons/rs2.png?n=50&d=WEST"
              }
            ],
            "index": 3,
            "maneuverNotes": [],
            "direction": 4,
            "narrative": "Merge onto Arlington Blvd/US-50 W via the ramp on the left.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/rs_merge_left_sm.gif",
            "distance": 1.374,
            "time": 124,
            "linkIds": [],
            "streets": [
              "Arlington Blvd",
              "US-50 W"
            ],
            "attributes": 0,
            "transportMode": "AUTO",
            "formattedTime": "00:02:04",
            "directionName": "South",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-4,38.890842,-77.077148,0,0|purple-5,38.874595,-77.085334,0,0|&center=38.882718499999996,-77.081241&zoom=9&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.077148,
              "lat": 38.890842
            },
            "turnType": 11
          },
          {
            "signs": [],
            "index": 4,
            "maneuverNotes": [],
            "direction": 1,
            "narrative": "Turn right onto Arlington Blvd.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/rs_right_sm.gif",
            "distance": 0.022,
            "time": 3,
            "linkIds": [],
            "streets": [
              "Arlington Blvd"
            ],
            "attributes": 0,
            "transportMode": "AUTO",
            "formattedTime": "00:00:03",
            "directionName": "North",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-5,38.874595,-77.085334,0,0|purple-6,38.874888999999996,-77.085411,0,0|&center=38.874742,-77.0853725&zoom=15&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.085334,
              "lat": 38.874595
            },
            "turnType": 2
          },
          {
            "signs": [],
            "index": 5,
            "maneuverNotes": [],
            "direction": 3,
            "narrative": "Turn right to stay on Arlington Blvd.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/rs_right_sm.gif",
            "distance": 0.083,
            "time": 9,
            "linkIds": [],
            "streets": [
              "Arlington Blvd"
            ],
            "attributes": 0,
            "transportMode": "AUTO",
            "formattedTime": "00:00:09",
            "directionName": "Northeast",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-6,38.874888999999996,-77.085411,0,0|purple-7,38.875598,-77.084228,0,0|&center=38.875243499999996,-77.0848195&zoom=14&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.085411,
              "lat": 38.874889
            },
            "turnType": 2
          },
          {
            "signs": [
              {
                "text": "27",
                "extraText": "",
                "direction": 1,
                "type": 545,
                "url": "http://icons.mqcdn.com/icons/rs545.png?n=27&d=NORTH"
              }
            ],
            "index": 6,
            "maneuverNotes": [],
            "direction": 1,
            "narrative": "Take VA-27 N/Washington Blvd N.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/rs_right_sm.gif",
            "distance": 1.029,
            "time": 85,
            "linkIds": [],
            "streets": [
              "VA-27 N",
              "Washington Blvd N"
            ],
            "attributes": 128,
            "transportMode": "AUTO",
            "formattedTime": "00:01:25",
            "directionName": "North",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-7,38.875598,-77.084228,0,0|purple-8,38.865337,-77.071159,0,0|&center=38.8704675,-77.0776935&zoom=10&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.084228,
              "lat": 38.875598
            },
            "turnType": 2
          },
          {
            "signs": [
              {
                "text": "395",
                "extraText": "",
                "direction": 4,
                "type": 1,
                "url": "http://icons.mqcdn.com/icons/rs1.png?n=395&d=SOUTH"
              }
            ],
            "index": 7,
            "maneuverNotes": [],
            "direction": 4,
            "narrative": "Merge onto I-395 S/Henry Shirley Memorial Hwy S toward Richmond.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/rs_merge_right_sm.gif",
            "distance": 1.121,
            "time": 77,
            "linkIds": [],
            "streets": [
              "I-395 S",
              "Henry Shirley Memorial Hwy S"
            ],
            "attributes": 128,
            "transportMode": "AUTO",
            "formattedTime": "00:01:17",
            "directionName": "South",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-8,38.865337,-77.071159,0,0|purple-9,38.850994,-77.07614099999999,0,0|&center=38.8581655,-77.07364999999999&zoom=9&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.071159,
              "lat": 38.865337
            },
            "turnType": 10
          },
          {
            "signs": [
              {
                "text": "7",
                "extraText": "",
                "direction": 0,
                "type": 1001,
                "url": "http://icons.mqcdn.com/icons/rs1001.png?n=7&d=RIGHT"
              }
            ],
            "index": 8,
            "maneuverNotes": [],
            "direction": 6,
            "narrative": "Take the VA-120/Glebe Rd exit, EXIT 7, toward Shirlington.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/rs_gr_exitright_sm.gif",
            "distance": 0.178,
            "time": 17,
            "linkIds": [],
            "streets": [],
            "attributes": 0,
            "transportMode": "AUTO",
            "formattedTime": "00:00:17",
            "directionName": "Southwest",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-9,38.850994,-77.07614099999999,0,0|purple-10,38.848869,-77.07800999999999,0,0|&center=38.8499315,-77.07707549999999&zoom=12&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.076141,
              "lat": 38.850994
            },
            "turnType": 14
          },
          {
            "signs": [
              {
                "text": "120",
                "extraText": "",
                "direction": 0,
                "type": 545,
                "url": "http://icons.mqcdn.com/icons/rs545.png?n=120"
              }
            ],
            "index": 9,
            "maneuverNotes": [],
            "direction": 2,
            "narrative": "Merge onto S Glebe Rd/VA-120 toward Marymount University.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/rs_merge_right_sm.gif",
            "distance": 0.403,
            "time": 59,
            "linkIds": [],
            "streets": [
              "S Glebe Rd",
              "VA-120"
            ],
            "attributes": 0,
            "transportMode": "AUTO",
            "formattedTime": "00:00:59",
            "directionName": "Northwest",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-10,38.848869,-77.07800999999999,0,0|purple-11,38.850356999999995,-77.083412,0,0|&center=38.849613,-77.080711&zoom=12&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.07801,
              "lat": 38.848869
            },
            "turnType": 10
          },
          {
            "signs": [
              {
                "text": "120",
                "extraText": "",
                "direction": 0,
                "type": 545,
                "url": "http://icons.mqcdn.com/icons/rs545.png?n=120"
              }
            ],
            "index": 10,
            "maneuverNotes": [],
            "direction": 5,
            "narrative": "Make a U-turn at 22nd St S onto S Glebe Rd/VA-120.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/rs_uturn_left_sm.gif",
            "distance": 0.161,
            "time": 28,
            "linkIds": [],
            "streets": [
              "S Glebe Rd",
              "VA-120"
            ],
            "attributes": 0,
            "transportMode": "AUTO",
            "formattedTime": "00:00:28",
            "directionName": "Southeast",
            "mapUrl": "http://open.mapquestapi.com/staticmap/v4/getmap?key=KEY&type=map&size=225,160&pois=purple-11,38.850356999999995,-77.083412,0,0|purple-12,38.848926,-77.08122999999999,0,0|&center=38.8496415,-77.082321&zoom=13&rand=113852568&session=55f0626d-01f2-0018-02b7-42c0-00163e1df8e2",
            "startPoint": {
              "lng": -77.083412,
              "lat": 38.850357
            },
            "turnType": 9
          },
          {
            "signs": [],
            "index": 11,
            "maneuverNotes": [],
            "direction": 0,
            "narrative": "2400 S GLEBE RD is on the right.",
            "iconUrl": "http://content.mapquest.com/mqsite/turnsigns/icon-dirs-end_sm.gif",
            "distance": 0,
            "time": 0,
            "linkIds": [],
            "streets": [],
            "attributes": 0,
            "transportMode": "AUTO",
            "formattedTime": "00:00:00",
            "directionName": "",
            "startPoint": {
              "lng": -77.08123,
              "lat": 38.848926
            },
            "turnType": -1
          }
        ],
        "hasFerry": false
      }
    ],
    "formattedTime": "00:07:25",
    "routeError": {
      "message": "",
      "errorCode": -400
    },
    "options": {
      "mustAvoidLinkIds": [],
      "drivingStyle": 2,
      "countryBoundaryDisplay": true,
      "generalize": 0,
      "narrativeType": "text",
      "locale": "en_US",
      "avoidTimedConditions": false,
      "destinationManeuverDisplay": true,
      "enhancedNarrative": false,
      "filterZoneFactor": -1,
      "timeType": 1,
      "maxWalkingDistance": -1,
      "routeType": "FASTEST",
      "transferPenalty": -1,
      "stateBoundaryDisplay": true,
      "walkingSpeed": -1,
      "maxLinkId": 0,
      "arteryWeights": [],
      "tryAvoidLinkIds": [],
      "unit": "M",
      "routeNumber": 0,
      "shapeFormat": "raw",
      "maneuverPenalty": -1,
      "useTraffic": false,
      "returnLinkDirections": false,
      "avoidTripIds": [],
      "manmaps": "true",
      "highwayEfficiency": 21,
      "sideOfStreetDisplay": true,
      "cyclingRoadFactor": 1,
      "urbanAvoidFactor": -1
    },
    "hasFerry": false
  },
  "info": {
    "copyright": {
      "text": "© 2018 MapQuest, Inc.",
      "imageUrl": "http://api.mqcdn.com/res/mqlogo.gif",
      "imageAltText": "© 2018 MapQuest, Inc."
    },
    "statuscode": 0,
    "messages": []
  }
}
 */
