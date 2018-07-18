using UnityEngine;
using System.Collections;

#pragma warning disable 0414
#pragma warning disable 0618

public enum WeatherIs {
	Clear = 0,
	Cloudy = 1,
	LightRain = 2,
	HeavyRain = 3,
	Storm = 4,
	Snow = 5,
	Blizard = 6,
	Fog = 7,
	IntroFog = 8,
}

public enum PartOfDay {
	Day = 0,
	Night = 1,
}

public enum ClimateIs {
	SouthernElin = 0,
	NorthernElin = 1,
	Ariya = 2,
	Elevir = 3,
	Korona = 4,
}

public class TodWeather : MonoBehaviour {

	public WeatherIs weatherIs;
	public PartOfDay nowItIs;
	public ClimateIs climateIs;

	public Material skyboxMaterial;
	private Material skyboxMaterialInstance;
	
	[Header("Particles")]
	public ParticleSystem fog;
	public ParticleSystem clouds;
	public ParticleSystem rains;
	public ParticleSystem snow;
	public ParticleSystem thunderLighting;
	public ParticleSystem stars;
	
	[Header("Celestials")]
	public Light sun;
	public GameObject skybox;
	public Transform skyboxFollow;

    [Header("Ambient and Lighting")]
    public Color currentLightOutside;
	public Color dayLight;
	public Color nightLight;
	public Color dayAmbient;
	public Color nightAmbient;
	public Color fogParticleDay;
	public Color fogParticleNight;
	public float skyExposure = 2f;
	public float sunShaftsDefault = 2f;
	[Space(5f)]
	public float secondsInFullDay = 120f;
	[Range(0,1)]
	public float currentTimeOfDay = 0;
	[HideInInspector]
	public float timeMultiplier = 1f;
	public Transform player;
	private TodClock clock;
    //[HideInInspector]
    public int currHour;
	private float cloudsColor;

	[Header("Weather Calulations")]
	[Tooltip("With this var we choose if we want weather to change automaticaly or not.")]
	public bool willWeatherChangeGeneric = true;
	[Tooltip("Weather Min Wait says what is the minimum time that must pass since last weather change to change the weather again.")]
	public float weatherMinWait = 60f;
	[Tooltip("Weather Max Wait says what is the maximum time that must pass since last weather change to change the weather again.")]
	public float weatherMaxWait = 360f;
	private float weatherCounter = 0f;
	public bool canChangeWeather = true;
	private int weatherRandom1 = 0;
	private int weatherRandom2 = 0;
	private int weatherRandom3 = 0;
	private int weatherChance = 0;

	[Header("Weather SFXs")]
	public AudioClip[] thunderSfx;
	public AudioClip[] rainSfx;
	public AudioClip[] windSfx;

    [HideInInspector]
    public bool inInterior;
    [HideInInspector]
    public Color interiorLight;

    private bool skyboxTimeReset;
    private float skyboxTime;
    private bool setSkyTime = false;

    void Start () {
		skyboxMaterialInstance = Instantiate (skyboxMaterial);
		RenderSettings.skybox = skyboxMaterialInstance;

		player = GameObject.Find ("[Player]").transform;

		weatherCounter = Random.Range (weatherMinWait, weatherMaxWait);
		clock = GetComponent<TodClock> ();

		stars.emissionRate = 0f;

		Time.timeScale = 1f;

		rains.enableEmission = true;
		clouds.enableEmission = true;
	}

	void Update () {
		int.TryParse(clock.currentHour, out currHour);

		if(skyboxFollow != null) {
			skybox.transform.position = skyboxFollow.position;
		}
        else {
            skybox.transform.position = player.position;
        }

		UpdateSunAndWeather();
		
		currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;
		
		if (currentTimeOfDay >= 1) {
			currentTimeOfDay = 0;
		}

		if(currHour >= 19) {
			skyboxTimeReset = true;
			FadeSkybox("Night");
		}
		else if(currHour >= 7 && currHour < 19) {
			skyboxTimeReset = true;
			FadeSkybox("Day");
		}

		skyboxMaterialInstance.SetFloat("_Exposure", skyExposure);

		if (willWeatherChangeGeneric == true && Time.timeScale < 0.25f) {
			CalculateTheWeather();
		}

        if (inInterior == false) {
            RenderSettings.ambientSkyColor = currentLightOutside;
        }
        else {
            RenderSettings.ambientSkyColor = interiorLight;
        }
    }

    void UpdateSunAndWeather() {
		sun.transform.localRotation = Quaternion.Euler ((currentTimeOfDay * -360f) - 90, 0, 0);

		if(weatherIs == WeatherIs.Clear) {
			if(sun.shadowStrength < 0.999f) {
				sun.shadowStrength += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			else {
				sun.shadowStrength = 1f;
			}

			if(nowItIs == PartOfDay.Day) {
				if(sun.intensity < 1.0f) {
					sun.intensity += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
				else {
					sun.intensity = 1f;
				}
			}

			thunderLighting.enableEmission = false;
			if(clouds.startColor != Color.white) {
				cloudsColor += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.01f;
				clouds.startColor = Color.Lerp(clouds.startColor, Color.white, cloudsColor);
			}
			else {
				cloudsColor = 0;
			}

			if(fog.emissionRate > 0f) {
				fog.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(rains.emissionRate > 0f) {
				rains.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(snow.emissionRate > 0f) {
				snow.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(clouds.emissionRate > 0f) {
				clouds.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}

			if(RenderSettings.fogDensity > 0f) {
                RenderSettings.fogDensity -= ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }

			if(skyExposure < 1.9f) {
				skyExposure += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
			}
			if(skyExposure > 2.1f) {
				skyExposure -= (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
			}
		}
		else if(weatherIs == WeatherIs.Cloudy) {
			if(sun.shadowStrength > 0.95f) {
				sun.shadowStrength -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			if(sun.shadowStrength < 0.9f) {
				sun.shadowStrength += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}

			if(nowItIs == PartOfDay.Day) {
				if(sun.intensity > 0.95f) {
					sun.intensity -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
				if(sun.intensity < 0.9f) {
					sun.intensity += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
			}

			thunderLighting.enableEmission = false;
			if(clouds.startColor != Color.white) {
				cloudsColor += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.01f;
				clouds.startColor = Color.Lerp(clouds.startColor, Color.white, cloudsColor);
			}
			else {
				cloudsColor = 0;
			}

			if(fog.emissionRate > 0f) {
				fog.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(rains.emissionRate > 0f) {
				rains.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(snow.emissionRate > 0f) {
				snow.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(clouds.emissionRate < 9f) {
				clouds.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(clouds.emissionRate > 11f) {
				clouds.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}

			if(RenderSettings.fogDensity < 0.001f) {
                RenderSettings.fogDensity += ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
			if(RenderSettings.fogDensity > 0.0015f) {
                RenderSettings.fogDensity -= ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }

			if(skyExposure < 1.9f) {
				skyExposure += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
			}
			if(skyExposure > 2.1f) {
				skyExposure -= (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
			}
		}
		else if(weatherIs == WeatherIs.LightRain) {
			if(sun.shadowStrength > 0.55f) {
				sun.shadowStrength -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			else if(sun.shadowStrength < 0.45f) {
				sun.shadowStrength += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}

			if(nowItIs == PartOfDay.Day) {
				if(sun.intensity > 0.55f) {
					sun.intensity -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
				else if(sun.intensity < 0.45f) {
					sun.intensity += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
			}

			thunderLighting.enableEmission = false;
			if(clouds.startColor != Color.gray) {
				cloudsColor += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.01f;
				clouds.startColor = Color.Lerp(clouds.startColor, Color.gray, cloudsColor);
			}
			else {
				cloudsColor = 0;
			}

			if(fog.emissionRate > 1f) {
					fog.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}

			if(clouds.emissionRate < 499f) {
					clouds.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(clouds.emissionRate > 501f) {
					clouds.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}

			if(clouds.emissionRate > 150f) {
				if(skyExposure > 1.6f) {
					skyExposure -= (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
				}
				if(skyExposure < 1.5f) {
					skyExposure += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
				}
				if(rains.emissionRate < 149f) {
					rains.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
				}
				if(rains.emissionRate > 151f) {
					rains.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
				}
			}
			if(snow.emissionRate > 0f) {
				snow.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(RenderSettings.fogDensity < 0.002f) {
                RenderSettings.fogDensity += ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
			if(RenderSettings.fogDensity > 0.0025f) {
                RenderSettings.fogDensity -= ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
		}
		else if(weatherIs == WeatherIs.HeavyRain) {
			if(sun.shadowStrength > 0.25f) {
				sun.shadowStrength -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			else if(sun.shadowStrength < 0.15f) {
				sun.shadowStrength += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}

			if(nowItIs == PartOfDay.Day) {
				if(sun.intensity > 0.25f) {
					sun.intensity -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
				else if(sun.intensity < 0.15f) {
					sun.intensity += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
			}

			thunderLighting.enableEmission = false;
			if(clouds.startColor != Color.gray) {
				cloudsColor += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.01f;
				clouds.startColor = Color.Lerp(clouds.startColor, Color.gray, cloudsColor);
			}
			else {
				cloudsColor = 0;
			}

			if(clouds.emissionRate < 599f) {
				clouds.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(clouds.emissionRate > 601f) {
				clouds.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}

			if(clouds.emissionRate > 70f) {
				if(fog.emissionRate <29f) {
					fog.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
				}
				if(fog.emissionRate > 31f) {
					fog.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
				}

				if(rains.emissionRate < 599f) {
					rains.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
				}
				if(rains.emissionRate > 601f) {
					rains.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
				}
				
				if(skyExposure > 1.1f) {
					skyExposure -= (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
				}
				if(skyExposure < 0.9f) {
					skyExposure += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
				}
			}
			if(snow.emissionRate > 0f) {
				snow.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(RenderSettings.fogDensity < 0.004f) {
                RenderSettings.fogDensity += ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
			if(RenderSettings.fogDensity > 0.0045f) {
                RenderSettings.fogDensity -= ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
		}
		else if(weatherIs == WeatherIs.Storm) {
			if(sun.shadowStrength > 0.1f) {
				sun.shadowStrength -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			else if(sun.shadowStrength < 0.1f) {
				sun.shadowStrength += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			
			if(nowItIs == PartOfDay.Day) {
				if(sun.intensity > 0.1f) {
					sun.intensity -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
				else if(sun.intensity < 0.1f) {
					sun.intensity += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
			}

			if(clouds.startColor != Color.gray) {
				cloudsColor += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.01f;
				clouds.startColor = Color.Lerp(clouds.startColor, Color.gray, cloudsColor);
			}
			else {
				cloudsColor = 0;
			}
					
			if(clouds.emissionRate < 699f) {
				clouds.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(clouds.emissionRate > 701f) {
				clouds.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}

			//We want to have clouds on the sky when rain starts to fall, sky starts to blacken and fog starts to spread
			if(clouds.emissionRate > 70f) {
				if(rains.emissionRate < 699f) {
					rains.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
				}
				if(rains.emissionRate > 701f) {
					rains.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
				}
				
				if(fog.emissionRate < 29f) {
					fog.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
				}
				if(fog.emissionRate > 31f) {
					fog.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
				}

				if(skyExposure > 1.1f) {
					skyExposure -= (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
				}
				if(skyExposure < 0.9f) {
					skyExposure += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
				}

				if(RenderSettings.fogDensity < 0.02f) {
                    RenderSettings.fogDensity += ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
                }
				if(RenderSettings.fogDensity > 0.025f) {
                    RenderSettings.fogDensity -= ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
                }
			}
			if(snow.emissionRate > 0f) {
				snow.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(clouds.emissionRate > 150f) {
				thunderLighting.enableEmission = true;
			}
			else {
				thunderLighting.enableEmission = false;
			}
		}
		else if(weatherIs == WeatherIs.Fog) {
			if(sun.shadowStrength > 0.25f) {
				sun.shadowStrength -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			if(sun.shadowStrength < 0.2f) {
				sun.shadowStrength += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			
			if(nowItIs == PartOfDay.Day) {
				if(sun.intensity > 0.3f) {
					sun.intensity -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
				if(sun.intensity < 0.3f) {
					sun.intensity += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
			}
			
			thunderLighting.enableEmission = false;
			if(clouds.startColor != Color.white) {
				cloudsColor += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.01f;
				clouds.startColor = Color.Lerp(clouds.startColor, Color.white, cloudsColor);
			}
			else {
				cloudsColor = 0;
			}
			
			if(fog.emissionRate < 29f) {
				fog.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(fog.emissionRate > 31f) {
				fog.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}

			if(rains.emissionRate > 0f) {
				rains.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(snow.emissionRate > 0f) {
				snow.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(clouds.emissionRate < 9f) {
				clouds.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(clouds.emissionRate > 11f) {
				clouds.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			
			if(RenderSettings.fogDensity < 0.04f) {
                RenderSettings.fogDensity += ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
			if(RenderSettings.fogDensity > 0.045f) {
                RenderSettings.fogDensity -= ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
			
			if(skyExposure < 0.3f) {
				skyExposure += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
			}
			if(skyExposure > 0.35f) {
				skyExposure -= (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
			}
		}
		else if(weatherIs == WeatherIs.Snow) {
			if(sun.shadowStrength > 0.25f) {
				sun.shadowStrength -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			if(sun.shadowStrength < 0.2f) {
				sun.shadowStrength += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			
			if(nowItIs == PartOfDay.Day) {
				if(sun.intensity > 0.3f) {
					sun.intensity -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
				if(sun.intensity < 0.3f) {
					sun.intensity += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
			}
			
			thunderLighting.enableEmission = false;
			if(clouds.startColor != Color.white) {
				cloudsColor += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.01f;
				clouds.startColor = Color.Lerp(clouds.startColor, Color.white, cloudsColor);
			}
			else {
				cloudsColor = 0;
			}
			
			if(fog.emissionRate < 10f) {
				fog.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(fog.emissionRate > 11f) {
				fog.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			
			if(rains.emissionRate > 0f) {
				rains.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(snow.emissionRate < 100f) {
				snow.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(snow.emissionRate > 101f) {
				snow.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(clouds.emissionRate < 699f) {
				clouds.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(clouds.emissionRate > 701f) {
				clouds.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			
			if(RenderSettings.fogDensity < 0.01f) {
                RenderSettings.fogDensity += ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
			if(RenderSettings.fogDensity > 0.015f) {
                RenderSettings.fogDensity -= ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
			
			if(skyExposure < 0.3f) {
				skyExposure += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
			}
			if(skyExposure > 0.35f) {
				skyExposure -= (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
			}
		}
		else if(weatherIs == WeatherIs.Blizard) {
			if(sun.shadowStrength > 0.25f) {
				sun.shadowStrength -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			if(sun.shadowStrength < 0.2f) {
				sun.shadowStrength += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			
			if(nowItIs == PartOfDay.Day) {
				if(sun.intensity > 0.3f) {
					sun.intensity -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
				if(sun.intensity < 0.3f) {
					sun.intensity += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
				}
			}
			
			thunderLighting.enableEmission = false;
			if(clouds.startColor != Color.white) {
				cloudsColor += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.01f;
				clouds.startColor = Color.Lerp(clouds.startColor, Color.white, cloudsColor);
			}
			else {
				cloudsColor = 0;
			}
			
			if(fog.emissionRate < 29f) {
				fog.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(fog.emissionRate > 30f) {
				fog.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			
			if(rains.emissionRate > 0f) {
				rains.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(snow.emissionRate < 250f) {
				snow.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(snow.emissionRate > 251f) {
				snow.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			if(clouds.emissionRate < 699f) {
				clouds.emissionRate += ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 10;
			}
			if(clouds.emissionRate > 701f) {
				clouds.emissionRate -= ((Time.deltaTime / (secondsInFullDay / 30)) * 20) * 100;
			}
			
			if(RenderSettings.fogDensity < 0.05f) {
                RenderSettings.fogDensity += ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
			if(RenderSettings.fogDensity > 0.055f) {
                RenderSettings.fogDensity -= ((Time.deltaTime / (secondsInFullDay / 30)) / 40);
            }
			
			if(skyExposure < 0.3f) {
				skyExposure += (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
			}
			if(skyExposure > 0.35f) {
				skyExposure -= (Time.deltaTime / (secondsInFullDay / 240f)) * 0.3f;
			}
		}
		else if(weatherIs == WeatherIs.IntroFog) {

			sun.shadowStrength = 0.2f;
			sun.intensity = 0.3f;
			fog.emissionRate = 30f;
			rains.emissionRate = 0f;
			clouds.emissionRate = 600f;
			clouds.startColor = Color.gray;
			RenderSettings.fogColor = dayAmbient;
			skyExposure = 0.3f;
            RenderSettings.fogDensity = 0.08f;
		}
	}

	//Weather Speed is not used only in Day'N'Night cycle, so that sun and moon and ambient light can change acording to the in game time.
	void FadeSkybox (string skyName) {

        if(setSkyTime == false) {
            skyboxTime = 0f;
            setSkyTime = true;
        }

        if (skyboxTime < 1f) {
            skyboxTime += (Time.deltaTime / (secondsInFullDay / 240f)) / 180f;
            //skyboxTime += Time.deltaTime;
        }

		if (skyName == "Day") {
			nowItIs = PartOfDay.Day;
            currentLightOutside = Color.Lerp(currentLightOutside, dayAmbient, skyboxTime);
            //RenderSettings.ambientLight = Color.Lerp (RenderSettings.ambientLight, dayAmbient, skyboxTime);
            RenderSettings.fogColor = Color.Lerp (RenderSettings.fogColor, dayAmbient, skyboxTime);
			fog.startColor = Color.Lerp (fog.startColor, fogParticleDay, skyboxTime);

			if(sun.intensity < 1f) {
				sun.intensity += (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			if(stars.emissionRate > 0f) {
				stars.emissionRate -= Time.deltaTime * 5f * (secondsInFullDay / 30);
			}

            if (currentLightOutside == dayAmbient) {
                skyboxTime = 0;
                skyboxTimeReset = false;
                setSkyTime = false;
            }
        }
		else if (skyName == "Night") {
			nowItIs = PartOfDay.Night;
            currentLightOutside = Color.Lerp(currentLightOutside, nightAmbient, skyboxTime);
            //RenderSettings.ambientLight = Color.Lerp (RenderSettings.ambientLight, nightAmbient, skyboxTime);
            RenderSettings.fogColor =  Color.Lerp (RenderSettings.fogColor, nightAmbient, skyboxTime);
			fog.startColor = Color.Lerp (fog.startColor, fogParticleNight, skyboxTime);

			if(sun.intensity > 0f) {
				sun.intensity -= (Time.deltaTime / (secondsInFullDay / 30)) * 10;
			}
			if(stars.emissionRate < 160f) {
				stars.emissionRate += Time.deltaTime * 5 * (secondsInFullDay / 30);
			}

            if (currentLightOutside == nightAmbient) {
                skyboxTime = 0;
                skyboxTimeReset = false;
                setSkyTime = false;
            }
        }
	}

	void CalculateTheWeather() {

		weatherChance = Random.Range (1, 1000);

		if (canChangeWeather == true) {
			if(weatherRandom1 == 1 && weatherRandom2 == 3 && (weatherRandom3 > 45 && weatherRandom3 < 65)) {
				if(weatherChance > 400 && weatherChance < 750) {
					if(climateIs == ClimateIs.SouthernElin) {
						if(weatherIs != WeatherIs.Clear) {
							weatherIs = WeatherIs.Clear;
						}
					}
					else if(climateIs == ClimateIs.NorthernElin) {
						if(weatherIs != WeatherIs.Fog) {
							weatherIs = WeatherIs.Fog;
						}
					}
				}
				else if(weatherChance > 150 && weatherChance < 399) {
					if(climateIs == ClimateIs.SouthernElin) {
						if(weatherIs != WeatherIs.Cloudy) {
							weatherIs = WeatherIs.Cloudy;
						}
					}
					else if(climateIs == ClimateIs.NorthernElin) {
						if(weatherIs != WeatherIs.Snow) {
							weatherIs = WeatherIs.Snow;
						}
					}
				}
				else if(weatherChance > 751 && weatherChance < 900) {
					if(climateIs == ClimateIs.SouthernElin) {
						if(weatherIs != WeatherIs.LightRain) {
							weatherIs = WeatherIs.LightRain;
						}
					}
					else if(climateIs == ClimateIs.NorthernElin) {
						if(weatherIs != WeatherIs.Snow) {
							weatherIs = WeatherIs.Snow;
						}
					}
				}
				else if(weatherChance > 0 && weatherChance < 149) {
					if(climateIs == ClimateIs.SouthernElin) {
						if(weatherIs != WeatherIs.HeavyRain) {
							weatherIs = WeatherIs.HeavyRain;
						}
					}
					else if(climateIs == ClimateIs.NorthernElin) {
						if(weatherIs != WeatherIs.Blizard) {
							weatherIs = WeatherIs.Blizard;
						}
					}
				}
				else if(weatherChance > 901 && weatherChance < 1001) {
					if(climateIs == ClimateIs.SouthernElin) {
						if(weatherIs != WeatherIs.Storm) {
							weatherIs = WeatherIs.Storm;
						}
					}
					else if(climateIs == ClimateIs.NorthernElin) {
						if(weatherIs != WeatherIs.Blizard) {
							weatherIs = WeatherIs.Blizard;
						}
					}
				}
				weatherCounter = Random.Range(weatherMinWait, weatherMaxWait);
				//We reset weatherRandom values so that the weather change will not occur when cooldown runs out because the "winning" values are set
				weatherRandom1 = 0;
				weatherRandom2 = 0;
				weatherRandom3 = 0;
				canChangeWeather = false;
			}
			else {
				weatherRandom1 = Random.Range(0, 2);
				weatherRandom2 = Random.Range(0, 20);
				weatherRandom3 = Random.Range(0, 100);
			}
		} 
		else {		
			if (weatherCounter > 0f) {
				weatherCounter -= Time.deltaTime;
			} 
			else {
				canChangeWeather = true;
			}
		}
	}
}
