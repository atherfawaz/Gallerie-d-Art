import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
import json
import random
import string
from datetime import date

http = urllib3.PoolManager()
f = open("art.txt", "w+", encoding="utf-8")

k = 0
j = 0
cc = 0
for i in range(200):
	r = http.request('GET', 'https://api.harvardartmuseums.org/object',
	    fields = {
	        'apikey': '73341a40-6387-11e9-bca8-351b1e7e09a0',
	        'fields': 'title,description,people.name,people.role,images.baseimageurl,medium,century',
	        'page': k,
	        'size': 100
	    })
	
	data = json.loads(r.data)
	for d in data['records']:
		if 'people' in d and d['description'] and len(d['description']) > 250 and d['title'] and 'images' in d and d['images'] and d['images'][0]['baseimageurl'] and 'medium' in d and d['medium'] and 'century' in d and d['century']:
			j = 0
			for p in d['people']:
				if p['role'] == 'Artist' and p['name'] and p['name'] != 'Unidentified Artist' and p['name'] != '':
					j += 1

			if j > 0:
				f.write(str(cc))
				f.write("=!=!=")
				aa = False
				for p in d['people']:
					if p['role'] == 'Artist' and p['name'] and p['name'] != 'Unidentified Artist' and p['name'] != '' and p['name'] != ' ':
						f.write(p['name'])
						aa = True
						if bool(aa):
							f.write("=!=!=")
							break

				f.write(d['title'])
				f.write("=!=!=")
				x = d['description']
				x = x.replace("\n", ' ')
				x = x.replace("\r", ' ')
				f.write(x)
				f.write("=!=!=")
				for a in d['medium']:
					f.write(a)
				f.write("=!=!=")
				f.write(d['century'])
				f.write("=!=!=")
				f.write(date.today().strftime('%Y-%m-%d'))
				f.write("=!=!=")
				f.write(str(random.randrange(30000, 500000, 500)))
				f.write("=!=!=")
				f.write(d['images'][0]['baseimageurl'])
				f.write("\n")
				cc += 1

	k += 20

f.close()